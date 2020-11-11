using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Services.Interfaces
{
    public interface IDataManager
    {
        IRepository<Category> CategoryRepository { get; set; }
        IRepository<Product> ProductRepository { get; set; }
        IRepository<PurchaseDetail> PurchaseDetailRepository { get; set; }
        IRepository<Order> OrderRepository { get; set; }
        IRepository<Review> ReviewRepository { get; set; }
    }
}