using System;
using System.Collections.Generic;
using System.Linq;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Test.Utils
{
    public static class DomainTestUtil
    {
        public static List<Category> GetTestCategories(int count = 5)
        {
            var categories = new List<Category>();

            for (var i = 0; i < count; i++)
            {
                categories.Add(new Category
                {
                    Id = Guid.NewGuid(), 
                    Desc = "Desc", 
                    Name = "Name",  
                    Products = GetTestProducts()
                });
            }

            return categories;
        }

        public static List<Product> GetTestProducts(int count = 5)
        {
            var products = new List<Product>();

            for (var i = 0; i < count; i++)
            {
                products.Add(new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Name", 
                        Desc = "Desc", 
                        ShortDesc = "ShortDesc",
                        ImagePath = "ImagePath", 
                        InStock = true, 
                        Price = 7, 
                        RatesCount = 1, 
                        Rating = 5
                    }
                );
            }

            return products;
        }
    }
}