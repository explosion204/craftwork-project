using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Web.Service.Validation
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize; // kb

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize * 1024)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Maximum allowed file size is {(float)_maxFileSize / 1024} KB.";
        }
    }
}