using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels;
using CraftworkProject.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexTest()
        {
            var mock = new Mock<IDataManager>();
            var categories = DomainTestUtil.GetTestCategories();
            mock.Setup(x => x.CategoryRepository.GetAllEntities())
                .Returns(categories);
            var homeController = new HomeController(mock.Object);

            var result = homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<HomeViewModel>(viewResult.Model);
            foreach (var item in model.AllCategories)
            {
                Assert.True(item.BestRatedProducts.Count <= 5);
            }
        }

        [Fact]
        public void SearchTest()
        {
            var searchViewModel = new SearchViewModel { Query = "test", Filter = "test" };
            var homeViewModel = new HomeViewModel
            {
                SearchViewModel = searchViewModel
            };
            var mock = new Mock<IDataManager>();
            var controller = new HomeController(mock.Object)
            {
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
            
            searchViewModel.Query = "test";
            var result = controller.Search(homeViewModel);
            
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/search?query=test&filter=test", redirectResult.Url);
        }
    }
}