using System;
using System.Linq;
using Craftwork_Project.Domain.Models;
using Craftwork_Project.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Craftwork_Project.Domain.Repositories.EntityFramework
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Product> GetAllProducts()
        {
            return context.Products;
        }

        public Product GetProduct(Guid id)
        {
            return context.Products.FirstOrDefault(x => x.Id == id);
        }

        public void SaveProduct(Product product)
        {
            if (product.Id == default)
            {
                context.Entry(product).State = EntityState.Added;
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteProduct(Guid id)
        {
            context.Products.Remove(new Product() { Id = id } );
            context.SaveChanges();
        }
    }
}