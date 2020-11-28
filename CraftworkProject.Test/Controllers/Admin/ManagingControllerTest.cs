using System;
using System.Collections.Generic;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Areas.Admin.Controllers;
using CraftworkProject.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CraftworkProject.Test.Controllers.Admin
{
    public class ManagingControllerTest
    {
        private ManagingController GetController()
        {
            var dataManagerMock = new Mock<IDataManager>();
            dataManagerMock.Setup(x => x.OrderRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestOrders());
            dataManagerMock.Setup(x => x.OrderRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestOrders(1)[0]);
            
            var notificationHubContextMock = new Mock<IHubContext<NotificationHub>>();
            var userConnectionManagerMock = new Mock<IUserConnectionManager>();
            var emailServiceMock = new Mock<IEmailService>();

            return new ManagingController(
                dataManagerMock.Object,
                notificationHubContextMock.Object,
                userConnectionManagerMock.Object,
                emailServiceMock.Object
            )
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
            Assert.IsType<List<Order>>(viewResult.Model);
        }

        [Fact]
        public void ConfirmOrderPost()
        {
            var controller = GetController();

            var result = controller.ConfirmOrder(Guid.NewGuid().ToString()).Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = true}), JsonConvert.SerializeObject(jsonResult.Value));
        }
        
        [Fact]
        public void CancelOrderPost()
        {
            var controller = GetController();

            var result = controller.CancelOrder(Guid.NewGuid().ToString()).Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = true}), JsonConvert.SerializeObject(jsonResult.Value));
        }
        
        [Fact]
        public void FinishOrderPost()
        {
            var controller = GetController();

            var result = controller.FinishOrder(Guid.NewGuid().ToString()).Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = true}), JsonConvert.SerializeObject(jsonResult.Value));
        }
    }
}