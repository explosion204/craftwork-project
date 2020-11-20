using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CraftworkProject.Domain;
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
    public class PurchaseDetailsControllerTest
    {
        private PurchaseDetailsController GetController()
        {
            var dataManagerMock = new Mock<IDataManager>();
            
            dataManagerMock.Setup(x => x.PurchaseDetailRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestPurchaseDetails());
            dataManagerMock.Setup(x => x.PurchaseDetailRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestPurchaseDetails(1)[0]);
            dataManagerMock.Setup(x => x.PurchaseDetailRepository.SaveEntity(It.IsAny<PurchaseDetail>()))
                .Returns<PurchaseDetail>(a => Guid.NewGuid());
            dataManagerMock.Setup(x => x.OrderRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestOrders());
            dataManagerMock.Setup(x => x.ProductRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestProducts());
            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestProducts(1)[0]);

            return new PurchaseDetailsController(dataManagerMock.Object)
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
            Assert.IsType<List<PurchaseDetail>>(viewResult.Model);
        }

        [Fact]
        public void CreateGetTest()
        {
            var controller = GetController();

            var result = controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PurchaseDetailViewModel>(viewResult.Model);
        }

        [Fact]
        public void CreatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new PurchaseDetailViewModel());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/purchasedetails", redirectResult.Url);
        }

        [Fact]
        public void CreatePostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Create(new PurchaseDetailViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PurchaseDetailViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdateGetTest()
        {
            var controller = GetController();

            var result = controller.Update(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PurchaseDetailViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdatePostTest()
        {
            var controller = GetController();

            var result = controller.Update(new PurchaseDetailViewModel());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/purchasedetails", redirectResult.Url);
        }

        [Fact]
        public void UpdatePostInvalidStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Update(new PurchaseDetailViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PurchaseDetailViewModel>(viewResult.Model);
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