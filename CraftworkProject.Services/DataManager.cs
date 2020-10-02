﻿using CraftworkProject.Domain;

namespace CraftworkProject.Services
{
    public class DataManager 
    {
        public IRepository<Category> Categories { get; set; }
        public IRepository<Product> Products { get; set; }
        public IRepository<PurchaseDetail> PurchaseDetails { get; set; }
        public IRepository<Order> Orders { get; set; }

        public DataManager(IRepository<Category> categoriesRepository, 
            IRepository<Product> productsRepository,
            IRepository<PurchaseDetail> purchaseDetailRepository,
            IRepository<Order> ordersRepository)
        {
            Categories = categoriesRepository;
            Products = productsRepository;
            PurchaseDetails = purchaseDetailRepository;
            Orders = ordersRepository;
        }
    }
}