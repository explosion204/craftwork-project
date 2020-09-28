using Craftwork_Project.Domain.Repositories.Interfaces;

namespace Craftwork_Project.Domain
{
    public class DataManager
    {
        public ICategoryRepository Categories { get; set; }
        public IProductRepository Products { get; set; }
        public IPurchaseDetailRepository PurchaseDetails { get; set; }

        public DataManager(ICategoryRepository categoriesRepository, 
            IProductRepository productsRepository,
            IPurchaseDetailRepository purchaseDetailRepository)
        {
            Categories = categoriesRepository;
            Products = productsRepository;
            PurchaseDetails = purchaseDetailRepository;
        }
    }
}