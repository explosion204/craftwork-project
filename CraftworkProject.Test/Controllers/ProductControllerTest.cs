using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Mocks;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class ProductControllerTest
    {
        private readonly User _testUser = DomainTestUtil.GetTestUsers(1)[0];
        private readonly Product _testProduct = DomainTestUtil.GetTestProducts(1)[0];
        private readonly List<Order> _testOrders = DomainTestUtil.GetTestOrders();

        private ProductController GetControllerWithAuthenticatedUser(bool returnsNullProduct = false)
        {
            var userManagerMock = new Mock<IUserManager>();
            var userManagerHelperMock = new Mock<IUserManagerHelper>();
            var dataManagerMock = new Mock<IDataManager>();

            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => returnsNullProduct ? null : _testProduct);

            dataManagerMock.Setup(x => x.ReviewRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestReviews());

            dataManagerMock.Setup(x => x.OrderRepository.GetAllEntities())
                .Returns(_testOrders);

            userManagerHelperMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(_testUser.Id);

            return new ProductController(dataManagerMock.Object, userManagerMock.Object, userManagerHelperMock.Object)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>()),
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new GenericIdentity("test")),
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        private ProductController GetControllerWithNotAuthenticatedUser(bool returnsNullProduct = false)
        {
            var userManagerMock = new Mock<IUserManager>();
            var userManagerHelperMock = new Mock<IUserManagerHelper>();
            var dataManagerMock = new Mock<IDataManager>();
            
            dataManagerMock.Setup(x => x.ProductRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => returnsNullProduct ? null : _testProduct);
            
            dataManagerMock.Setup(x => x.ReviewRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestReviews());

            var identityMock = new Mock<GenericIdentity>("test");
            identityMock.SetupGet(x => x.IsAuthenticated).Returns(false);

            return new ProductController(dataManagerMock.Object, userManagerMock.Object, userManagerHelperMock.Object)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>()),
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(identityMock.Object),
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        private static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
        {
            var requestFeature = new HttpRequestFeature();
            var featureCollection = new FeatureCollection();

            requestFeature.Headers = new HeaderDictionary {{ HeaderNames.Cookie, new StringValues(key + "=" + value) }};
            featureCollection.Set<IHttpRequestFeature>(requestFeature);
            var cookiesFeature = new RequestCookiesFeature(featureCollection);

            return cookiesFeature.Cookies;
        }
        
        [Fact]
        public void IndexGetNotFoundTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(returnsNullProduct: true);

            var result = controller.Index(Guid.NewGuid());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/error/404", redirectResult.Url);
        }

        [Fact]
        public void IndexGetWithModelErrorsTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();
            controller.TempData["errors"] = new []
            {
                "title", "text"
            };

            var result = controller.Index(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ProductViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount == 2);
        }
        
        [Fact]
        public void IndexGetNotAuthenticatedTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();
       
            var result = controller.Index(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductViewModel>(viewResult.Model);
            Assert.False(model.ReviewSubmitAllowed);
            Assert.True(controller.ModelState.ErrorCount == 0);
        }
        
        [Fact]
        public void IndexGetAuthenticatedReviewingAllowedTest()
        {
            var controller = GetControllerWithAuthenticatedUser();
            _testOrders[0].PurchaseDetails[0].Product = _testProduct;
            _testOrders[0].User = _testUser;
            _testOrders[0].Finished = true;

            var result = controller.Index(_testProduct.Id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductViewModel>(viewResult.Model);
            Assert.True(model.ReviewSubmitAllowed);
            Assert.True(controller.ModelState.ErrorCount == 0);
        }
        
        [Fact]
        public void IndexGetAuthenticatedReviewingNotAllowedTest()
        {
            var controller = GetControllerWithAuthenticatedUser();
            _testOrders[0].PurchaseDetails[0].Product = _testProduct;
            _testOrders[0].User = _testUser;
            _testOrders[0].Finished = false;

            var result = controller.Index(_testProduct.Id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductViewModel>(viewResult.Model);
            Assert.False(model.ReviewSubmitAllowed);
            Assert.True(controller.ModelState.ErrorCount == 0);
        }

        [Fact]
        public void SubmitReviewPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();
            var productId = Guid.NewGuid();
            controller.Request.Cookies = MockRequestCookieCollection("userRating", "5");
            
            var result = controller.SubmitReview(new ProductViewModel
            {
                ProductId = productId
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal($"/product?id={productId}", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 0);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }
        
        [Fact]
        public void SubmitReviewPostInvalidModelStateTest()
        {
            var controller = GetControllerWithAuthenticatedUser();
            var productId = Guid.NewGuid();
            controller.ModelState.AddModelError("errorName", "error");

            var result = controller.SubmitReview(new ProductViewModel
            {
                ProductId = productId
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal($"/product?id={productId}", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 1);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }
    }
}