using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class OrdersControllerTest
    {
        private OrdersController GetController()
        {
            var userManagerMock = new Mock<IUserManager>();
            var dataManagerMock = new Mock<IDataManager>();

            userManagerMock.Setup(x => x.FindUserByName(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(DomainTestUtil.GetTestUsers(1)[0]));

            dataManagerMock.Setup(x => x.OrderRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestOrders());

            dataManagerMock.Setup(x => x.OrderRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestOrders(1)[0]);

            return new OrdersController(dataManagerMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new GenericIdentity("test"))
                    }
                }
            };
        }

        [Fact]
        public void PendingOrdersGetTest()
        {
            var controller = GetController();

            var result = controller.PendingOrders().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Order>>(viewResult.Model);
            Assert.True((int)controller.ViewData["pendingOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["canceledOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["finishedOrdersCount"] == 0);
        }
        
        [Fact]
        public void CanceledOrdersGetTest()
        {
            var controller = GetController();

            var result = controller.CanceledOrders().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Order>>(viewResult.Model);
            Assert.True((int)controller.ViewData["pendingOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["canceledOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["finishedOrdersCount"] == 0);
        }
        
        [Fact]
        public void FinishedOrdersGetTest()
        {
            var controller = GetController();

            var result = controller.FinishedOrders().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Order>>(viewResult.Model);
            Assert.True((int)controller.ViewData["pendingOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["canceledOrdersCount"] == 0);
            Assert.True((int)controller.ViewData["finishedOrdersCount"] == 0);
        }

        [Fact]
        public void GetOrderGetTest()
        {
            var controller = GetController();

            var result = controller.GetOrder(Guid.NewGuid().ToString());
            Assert.IsType<JsonResult>(result);
        }
    }
}