using System;
using System.Linq;
using Craftwork_Project.Domain.Models;

namespace Craftwork_Project.Domain.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAllProducts();
        Product GetProduct(Guid id);
        void SaveProduct(Product product);
        void DeleteProduct(Guid id);
    }
}