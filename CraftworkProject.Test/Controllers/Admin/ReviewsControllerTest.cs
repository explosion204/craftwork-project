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
    public class ReviewsControllerTest
    {
        private readonly List<Product> _testProducts = DomainTestUtil.GetTestProducts();
        private readonly List<Review> _testReviews = DomainTestUtil.GetTestReviews();
        private readonly User _testUser = DomainTestUtil.GetTestUsers(1)[0];
        
        private ReviewsController GetController()
        {
            var dataManagerMock = new Mock<IDataManager>();
            dataManagerMock.Setup(x => x.ReviewRepository.GetAllEntities())
                .Returns(_testReviews);
            dataManagerMock.Setup(x => x.ReviewRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => DomainTestUtil.GetTestReviews(1)[0]);
            dataManagerMock.Setup(x => x.ReviewRepository.SaveEntity(It.IsAny<Review>()))
                .Returns<Review>(a => Guid.NewGuid());
            dataManagerMock.Setup(x => x.ProductRepository.GetAllEntities())
                .Returns(_testProducts);
            
            var userManagerMock = new Mock<IUserManager>();
            userManagerMock.Setup(x => x.GetAllUsers())
                .Returns(DomainTestUtil.GetTestUsers());
            userManagerMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
                .Returns<Guid>(a => Task.FromResult(_testUser));

            return new ReviewsController(dataManagerMock.Object, userManagerMock.Object)
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
            Assert.IsType<List<Review>>(viewResult.Model);
        }

        [Fact]
        public void CreateGetTest()
        {
            var controller = GetController();

            var result = controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
        }

        [Fact]
        public void CreatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new ReviewViewModel
            {
                Rating = 5
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/reviews", redirectResult.Url);
        }
        
        [Fact]
        public void CreatePostRatingOutOfRangeTest()
        {
            var controller = GetController();

            var result = controller.Create(new ReviewViewModel
            {
                Rating = 6
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
            
            result = controller.Create(new ReviewViewModel
            {
                Rating = 0
            }).Result;
            viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void CreatePostReviewAlreadyExistsTest()
        {
            var controller = GetController();
            _testReviews[0].Product.Id = _testProducts[0].Id;
            _testReviews[0].User.Id = _testUser.Id;

            var result = controller.Create(new ReviewViewModel
            {
                ProductId = _testProducts[0].Id,
                UserId = _testUser.Id,
                Rating = 5
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
        }

        [Fact]
        public void CreatePostInvalidModelStateTest()
        {
            var controller = GetController();
            controller.ModelState.AddModelError("errorName", "name");

            var result = controller.Create(new ReviewViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdateGetTest()
        {
            var controller = GetController();

            var result = controller.Update(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
        }

        [Fact]
        public void UpdatePostTest()
        {
            var controller = GetController();

            var result = controller.Create(new ReviewViewModel
            {
                Rating = 5
            }).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/admin/reviews", redirectResult.Url);
        }

        [Fact]
        public void UpdatePostRatingOutOfRange()
        {
            var controller = GetController();

            var result = controller.Update(new ReviewViewModel
            {
                Rating = 6
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
            
            result = controller.Update(new ReviewViewModel
            {
                Rating = 0
            }).Result;
            viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ReviewViewModel>(viewResult.Model);
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