using System;
using System.Collections.Generic;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Areas.Admin.Controllers;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CraftworkProject.Test.Controllers.Admin
{
    public class ProductsControllerTest
    {
        private ProductsController GetController()
        {
            var testProduct = DomainTestUtil.GetTestProducts(1)[0];
            testProduct.Category = DomainTestUtil.GetTestCategories(1)[0];
            var dataManagerMock = new Mock<IDataManager>();
            
            dataManagerMock.Setup(x => x.ProductRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestProducts());
            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => testProduct);
            dataManagerMock.Setup(x => x.ProductRepository.SaveEntity(It.IsAny<Product>()))
                .Returns<Product>(a => Guid.NewGuid());
            dataManagerMock.Setup(x => x.CategoryRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestCategories());
            dataManagerMock.Setup(x => x.CategoryRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestCategories(1)[0]);
            
            var imageServiceMock = new Mock<IImageService>();
            var environmentMock = new Mock<IWebHostEnvironment>();

            return new ProductsController(dataManagerMock.Object, imageServiceMock.Object, environmentMock.Object)
            {
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        [Fact]
        public void IndexGetTest()
        {
            var controller = GetController();

            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Product>>(viewResult.Model);
        }

        [Fact]
        public void CreateGetTest()
        {
            var controller = GetController();

            var result = controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void CreatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new ProductViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/products", redirectResult.Url);
        }

        [Fact]
        public void CreatePostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Create(new ProductViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdateGetTest()
        {
            var controller = GetController();

            var result = controller.Update(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdatePostTest()
        {
            var controller = GetController();

            var result = controller.Update(new ProductViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/products", redirectResult.Url);
        }

        [Fact]
        public void UpdatePostInvalidStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Update(new ProductViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void DeletePostTest()
        {
            var controller = GetController();

            var result = controller.Delete(Guid.NewGuid().ToString());
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = true}), JsonConvert.SerializeObject(jsonResult.Value));
        }
    }
}