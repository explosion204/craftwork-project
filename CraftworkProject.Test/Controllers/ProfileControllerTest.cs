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
using CraftworkProject.Web.ViewModels.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class ProfileControllerTest
    {
        private ProfileController GetController(
            bool changeablePassword = true,
            bool confirmablePhoneNumber = true,
            bool findsByPhoneNumber = true
        )
        {
            var testUser = DomainTestUtil.GetTestUsers(1)[0];
            
            var dataManagerMock = new Mock<IDataManager>();
            var userManagerMock = new Mock<IUserManager>();
            var userManagerHelperMock = new Mock<IUserManagerHelper>();
            var smsServiceMock = new Mock<ISmsService>();
            var imageServiceMock = new Mock<IImageService>();
            var environmentMock = new Mock<IWebHostEnvironment>();

            dataManagerMock.Setup(x => x.OrderRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestOrders());
            
            userManagerMock.Setup(x => x.FindUserByName(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(testUser));
            
            userManagerMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
                .Returns<Guid>(a => Task.FromResult(DomainTestUtil.GetTestUsers(1)[0]));

            userManagerMock.Setup(x => x.FindUserByPhoneNumber(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(findsByPhoneNumber ? testUser : null));

            userManagerMock.Setup(x => x.ChangeUserPassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<Guid, string, string>((a, b, c) => Task.FromResult(changeablePassword));

            userManagerMock.Setup(x => x.ConfirmPhoneNumber(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns<Guid, string>((a, b) => Task.FromResult(confirmablePhoneNumber));
            
            return new ProfileController(
                dataManagerMock.Object, 
                userManagerMock.Object,
                userManagerHelperMock.Object,
                smsServiceMock.Object,
                imageServiceMock.Object, 
                environmentMock.Object
            )
            {
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object,
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>()),
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
        public void IndexGetTest()
        {
            var controller = GetController();
            controller.TempData["errors"] = null;

            var result = controller.Index().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
            
            Assert.NotNull(viewResult.ViewData["pendingOrdersCount"]);
            Assert.NotNull(viewResult.ViewData["canceledOrdersCount"]);
            Assert.NotNull(viewResult.ViewData["finishedOrdersCount"]);
        }

        [Fact]
        public void IndexGetWithModelErrorsTest()
        {
            var controller = GetController();
            controller.TempData["errors"] = new []
            {
                "first", "last", "file", "current password", "password"
            };
            
            var result = controller.Index().Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
            
            Assert.NotNull(viewResult.ViewData["pendingOrdersCount"]);
            Assert.NotNull(viewResult.ViewData["canceledOrdersCount"]);
            Assert.NotNull(viewResult.ViewData["finishedOrdersCount"]);
            Assert.True(controller.ModelState.ErrorCount == 6);
        }

        [Fact]
        public void ChangePersonalInfoPostTest()
        {
            var controller = GetController();

            var result = controller.ChangePersonalInfo(new UserViewModel { ProfilePicture = null}).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
        }
        
        [Fact]
        public void ChangePersonalInfoPostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "error");

            var result = controller.ChangePersonalInfo(new UserViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 1);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }
        
        [Fact]
        public void ChangePasswordPostTest()
        {
            var controller = GetController();

            var result = controller.ChangePassword(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
        }
        
        [Fact]
        public void ChangePasswordPostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "error");
            
            var result = controller.ChangePassword(new UserViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 1);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }

        [Fact]
        public void ChangePasswordPostPasswordsDoNotMatchTest()
        {
            var controller = GetController();

            var result = controller.ChangePassword(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password_"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 2);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }

        [Fact]
        public void ChangePasswordPostUnableToChangeTest()
        {
            var controller = GetController(changeablePassword: false);

            var result = controller.ChangePassword(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
            Assert.True(controller.ModelState.ErrorCount == 1);
            Assert.NotNull(controller.TempData["errors"]);
            Assert.True(((List<string>)controller.TempData["errors"]).Count == controller.ModelState.ErrorCount);
        }

        [Fact]
        public void ChangePhoneNumberGetTest()
        {
            var controller = GetController();

            var result = controller.ChangePhoneNumber();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangePhoneNumberViewModel>(viewResult.Model);
        }

        [Fact]
        public void ChangePhoneNumberPostTest()
        {
            var controller = GetController(findsByPhoneNumber: false);

            var result = controller.ChangePhoneNumber(new ChangePhoneNumberViewModel()
            {
                UserId = Guid.NewGuid(),
                Code = "code",
                PhoneNumber = "test"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
        }
        
        [Fact]
        public void ChangePhoneNumberInvalidModelStateTest()
        {
            var controller = GetController(findsByPhoneNumber: false);
            controller.ModelState.AddModelError("errorName", "error");

            var result = controller.ChangePhoneNumber(new ChangePhoneNumberViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangePhoneNumberViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void ChangePhoneNumberPostDuplicatePhoneNumberTest()
        {
            var controller = GetController(findsByPhoneNumber: true);
            
            var result = controller.ChangePhoneNumber(new ChangePhoneNumberViewModel()
            {
                UserId = Guid.NewGuid(),
                Code = "code",
                PhoneNumber = "test"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangePhoneNumberViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void ChangePhoneNumberPostEmptyCodeTest()
        {
            var controller = GetController(findsByPhoneNumber: false);

            var result = controller.ChangePhoneNumber(new ChangePhoneNumberViewModel()
            {
                UserId = Guid.NewGuid(),
                Code = string.Empty,
                PhoneNumber = "test"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangePhoneNumberViewModel>(viewResult.Model);
        }

        [Fact]
        public void ChangePhoneNumberPostInvalidCodeTest()
        {
            var controller = GetController(confirmablePhoneNumber: false, findsByPhoneNumber: false);
            
            var result = controller.ChangePhoneNumber(new ChangePhoneNumberViewModel()
            {
                UserId = Guid.NewGuid(),
                Code = "code",
                PhoneNumber = "test"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangePhoneNumberViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount == 1);
        }
    }
}