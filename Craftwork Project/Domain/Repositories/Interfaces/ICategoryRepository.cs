using System;
using System.Linq;
using Craftwork_Project.Domain.Models;

namespace Craftwork_Project.Domain.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAllCategories();
        Category GetCategory(Guid id);
        void SaveCategory(Category category);
        void DeleteCategory(Guid id);
    }
}