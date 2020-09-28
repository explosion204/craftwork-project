using System;
using System.Linq;
using Craftwork_Project.Domain.Models;
using Craftwork_Project.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Craftwork_Project.Domain.Repositories.EntityFramework
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;

        public EFCategoryRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Category> GetAllCategories()
        {
            return context.Categories;
        }

        public Category GetCategory(Guid id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }

        public void SaveCategory(Category category)
        {
            if (category.Id == default)
            {
                context.Entry(category).State = EntityState.Added;
            }
            else
            {
                context.Entry(category).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteCategory(Guid id)
        {
            context.Categories.Remove(new Category() { Id = id } );
            context.SaveChanges();
        }
    }
}