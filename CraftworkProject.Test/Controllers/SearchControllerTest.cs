using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class SearchControllerTest
    {
        [Fact]
        public void IndexGetTest()
        {
            var dataManagerMock = new Mock<IDataManager>();
            dataManagerMock.Setup(x => x.ProductRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestProducts());
            dataManagerMock.Setup(x => x.CategoryRepository.GetAllEntities())
                .Returns(DomainTestUtil.GetTestCategories());
            
            var controller = new SearchController(dataManagerMock.Object);

            var result = controller.Index("query", "filter");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/ListView.cshtml", viewResult.ViewName);
            var model = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.True(model.Products.Count < 9);
        }
    }
}