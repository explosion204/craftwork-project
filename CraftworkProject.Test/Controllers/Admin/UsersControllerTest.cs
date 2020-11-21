using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Areas.Admin.Controllers;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CraftworkProject.Test.Controllers.Admin
{
    public class UsersControllerTest
    {
        private readonly List<User> _testUsers = DomainTestUtil.GetTestUsers();
        
        private UsersController GetController(bool creatableUser = true, bool updatableUser = true)
        {
            var userManagerMock = new Mock<IUserManager>();
            var userManagerHelperMock = new Mock<IUserManagerHelper>();
            
            userManagerMock.Setup(x => x.GetAllUsers())
                .Returns(_testUsers);
            userManagerMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
                .Returns<Guid>(a => Task.FromResult(_testUsers[0]));
            userManagerMock.Setup(x => x.FindUserByName(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(_testUsers[1]));
            userManagerMock.Setup(x => x.CreateUser(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns<User, string, Guid>((a, b, c) => Task.FromResult(creatableUser));
            userManagerMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .Returns<User>(a => Task.FromResult(updatableUser));
            userManagerHelperMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns<ClaimsPrincipal>(a => _testUsers[0].Id);
            
            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.SetupGet(x => x.WebRootPath).Returns(Directory.GetCurrentDirectory());
            
            var imageServiceMock = new Mock<IImageService>();

            return new UsersController(
                userManagerMock.Object,
                userManagerHelperMock.Object,
                environmentMock.Object, 
                imageServiceMock.Object
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
            Assert.IsType<List<User>>(viewResult.Model);
        }

        [Fact]
        public void CreateGetTest()
        {
            var controller = GetController();

            var result = controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }

        [Fact]
        public void CreatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/users", redirectResult.Url);
        }
        
        [Fact]
        public void CreatePostNullOrEmptyNewPasswordTest()
        {
            var controller = GetController();

            var result = controller.Create(new UserViewModel
            {
                NewPassword = "",
                ConfirmNewPassword = "password"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void CreatePostNullOrEmptyConfirmNewPasswordTest()
        {
            var controller = GetController();

            var result = controller.Create(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = ""
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void CreatePostPasswordDoNotMatchTest()
        {
            var controller = GetController();

            var result = controller.Create(new UserViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password_"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount == 2);
        }

        [Fact]
        public void UpdateGetTest()
        {
            var controller = GetController();

            var result = controller.Update(Guid.NewGuid()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdatePostTest()
        {
            var controller = GetController();
            var formFileMock = new Mock<IFormFile>();
            formFileMock.SetupGet(x => x.FileName).Returns("test");
            _testUsers[0].ProfilePicture = "test";

            var result = controller.Update(new UserViewModel
            {
                ProfilePicture = formFileMock.Object,
                
                NewPassword = "password",
                ConfirmNewPassword = "password"
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/users", redirectResult.Url);
        }

        [Fact]
        public void UpdatePostPasswordsDoNotMatch()
        {
            var controller = GetController();
            var formFileMock = new Mock<IFormFile>();
            formFileMock.SetupGet(x => x.FileName).Returns("test");
            _testUsers[1].ProfilePicture = "test";

            var result = controller.Update(new UserViewModel
            {
                ProfilePicture = formFileMock.Object,
                NewPassword = "password",
                ConfirmNewPassword = "password_"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void UpdatePostInvalidStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Update(new UserViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<UserViewModel>(viewResult.Model);
        }

        [Fact]
        public void DeletePostSameUserTest()
        {
            var controller = GetController();

            var result = controller.Delete(_testUsers[0].Id.ToString()).Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = false}), JsonConvert.SerializeObject(jsonResult.Value));
        }
        
        [Fact]
        public void DeletePostNotSameUserTest()
        {
            var controller = GetController();

            var result = controller.Delete(Guid.NewGuid().ToString()).Result;
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new {success = true}), JsonConvert.SerializeObject(jsonResult.Value));
        }
    }
}