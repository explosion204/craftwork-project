using System;
using System.Collections.Generic;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Areas.Admin.Controllers;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CraftworkProject.Test.Controllers.Admin
{
    public class CategoriesControllerTest
    {
        private CategoriesController GetController()
        {
            var dataManagerMock = new Mock<IDataManager>();
            dataManagerMock.Setup(x => x.CategoryRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestCategories());
            dataManagerMock.Setup(x => x.CategoryRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestCategories(1)[0]);
            dataManagerMock.Setup(x => x.CategoryRepository.SaveEntity(It.IsAny<Category>()))
                .Returns<Category>(a => Guid.NewGuid());

            return new CategoriesController(dataManagerMock.Object)
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
            Assert.IsType<List<Category>>(viewResult.Model);
        }

        [Fact]
        public void CreateGetTest()
        {
            var controller = GetController();

            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new Category());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/categories", redirectResult.Url);
        }

        [Fact]
        public void CreatePostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Create(new Category());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Category>(viewResult.Model);
        }

        [Fact]
        public void UpdateGetTest()
        {
            var controller = GetController();

            var result = controller.Update(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Category>(viewResult.Model);
        }

        [Fact]
        public void UpdatePostTest()
        {
            var controller = GetController();

            var result = controller.Update(new Category());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/categories", redirectResult.Url);
        }

        [Fact]
        public void UpdatePostInvalidStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Update(new Category());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Category>(viewResult.Model);
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