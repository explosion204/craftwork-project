using System;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class CategoryControllerTest
    {
        private CategoryController GetController(bool returnsNullCategory = false)
        {
            var dataManagerMock = new Mock<IDataManager>();
            dataManagerMock.Setup(x => x.CategoryRepository.GetEntity(It.IsAny<Guid>()))
                .Returns<Guid>(a => returnsNullCategory ? null : DomainTestUtil.GetTestCategories(1)[0]);

            return new CategoryController(dataManagerMock.Object);
        }

        [Fact]
        public void IndexGetTest()
        {
            var controller = GetController();

            var result = controller.Index(Guid.NewGuid());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/ListView.cshtml", viewResult.ViewName);
            var model = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.True(model.Products.Count < 9);
        }

        [Fact]
        public void IndexGetCategoryNotFoundTest()
        {
            var controller = GetController(true);

            var result = controller.Index(Guid.NewGuid());
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/error/404", redirectResult.Url);
        }
    }
}