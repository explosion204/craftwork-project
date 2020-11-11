using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;

namespace CraftworkProject.Services.Implementations
{
    public class DataManager : IDataManager
    {
        public IRepository<Category> CategoryRepository { get; set; }
        public IRepository<Product> ProductRepository { get; set; }
        public IRepository<PurchaseDetail> PurchaseDetailRepository { get; set; }
        public IRepository<Order> OrderRepository { get; set; }
        public IRepository<Review> ReviewRepository { get; set; }

        public DataManager(IRepository<Category> categoriesRepository, 
            IRepository<Product> productsRepository,
            IRepository<PurchaseDetail> purchaseDetailRepository,
            IRepository<Order> ordersRepository,
            IRepository<Review> reviewRepository)
        {
            CategoryRepository = categoriesRepository;
            ProductRepository = productsRepository;
            PurchaseDetailRepository = purchaseDetailRepository;
            OrderRepository = ordersRepository;
            ReviewRepository = reviewRepository;
        }
    }
}