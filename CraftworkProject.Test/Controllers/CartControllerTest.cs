using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Mocks;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class CartControllerTest
    {
        private readonly string _testJsonString =
            $"[\"{Guid.NewGuid().ToString()}\", \"{Guid.NewGuid().ToString()}\", \"{Guid.NewGuid().ToString()}\"]";
        private CartController GetControllerWithAuthenticatedUser()
        {
            var testProduct = DomainTestUtil.GetTestProducts(1)[0];
            var userManagerMock = new Mock<IUserManager>();
            var dataManagerMock = new Mock<IDataManager>();
            
            userManagerMock.Setup(x => x.FindUserByName(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(DomainTestUtil.GetTestUsers(1)[0]));
            
            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns(testProduct);

            dataManagerMock.Setup(x => x.OrderRepository.SaveEntity(It.IsAny<Order>()))
                .Returns<Order>(a => Guid.NewGuid());

            dataManagerMock.Setup(x => x.PurchaseDetailRepository.SaveEntity(It.IsAny<PurchaseDetail>()))
                .Returns<PurchaseDetail>(a => Guid.NewGuid());

            return new CartController(dataManagerMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new GenericIdentity("test")),
                        Session = new MockHttpSession
                        {
                            ["cart"] = _testJsonString
                        }
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        private CartController GetControllerWithNotAuthenticatedUser()
        {
            var testProduct = DomainTestUtil.GetTestProducts(1)[0];
            var userManagerMock = new Mock<IUserManager>();
            var dataManagerMock = new Mock<IDataManager>();

            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns(testProduct);
            
            var identityMock = new Mock<GenericIdentity>("test");
            identityMock.SetupGet(x => x.IsAuthenticated).Returns(false);

            return new CartController(dataManagerMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(identityMock.Object),
                        Session = new MockHttpSession
                        {
                            ["cart"] = _testJsonString
                        }
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        [Fact]
        public void IndexGetAuthenticatedText()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Index().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CartViewModel>(viewResult.Model);
            Assert.True(model.Quantities.Count == 3);
            Assert.True(model.Products.Count == 3);
            Assert.True(model.MakeOrderAllowed);
        }
        
        [Fact]
        public void IndexGetNotAuthenticatedText()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Index().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CartViewModel>(viewResult.Model);
            Assert.True(model.Quantities.Count == 3);
            Assert.True(model.Products.Count == 3);
            Assert.False(model.MakeOrderAllowed);
        }

        [Fact]
        public void SetCartPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.SetCart("[]");
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("{ success = True }", jsonResult.Value.ToString());
            Assert.Equal(JArray.Parse(_testJsonString), JArray.Parse(controller.HttpContext.Session.GetString("cart")));
        }

        [Fact]
        public void MakeOrderPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.MakeOrder().Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("{ success = True }", jsonResult.Value.ToString());
            Assert.Equal("[]", controller.HttpContext.Session.GetString("cart"));
        }

        [Fact]
        public void SuccessGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Success();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DeletePostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Delete(JArray.Parse(_testJsonString)[0].ToString());
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("{ success = True }", jsonResult.Value.ToString());
            Assert.True(JArray.Parse(controller.HttpContext.Session.GetString("cart")).Count == 2);
        }
    }
}