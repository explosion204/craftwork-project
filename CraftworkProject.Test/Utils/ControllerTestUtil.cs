using CraftworkProject.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;

namespace CraftworkProject.Test.Utils
{
    public static class ControllerTestUtil
    {
        public static Mock<IObjectModelValidator> GetObjectModelValidatorMock()
        {
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(x => x.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));

            return objectValidator;
        }
    }
}