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

        public static List<Order> GetTestOrders(int count = 5)
        {
            var orders = new List<Order>();

            for (var i = 0; i < count; i++)
            {
                orders.Add(new Order
                {
                    Id = Guid.NewGuid(),
                    User = GetTestUsers(1)[0],
                    PurchaseDetails = GetTestPurchaseDetails(),
                    Canceled = false,
                    Processed = false,
                    Finished = false,
                    Created = DateTime.Now
                });
            }

            return orders;
        }

        public static List<PurchaseDetail> GetTestPurchaseDetails(int count = 5)
        {
            var details = new List<PurchaseDetail>();

            for (var i = 0; i < count; i++)
            {
                details.Add(new PurchaseDetail()
                {
                    Id = Guid.NewGuid(),
                    Product = GetTestProducts(1)[0],
                    OrderId = Guid.NewGuid(),
                    Amount = 1
                });
            }

            return details;
        }

        public static List<User> GetTestUsers(int count = 5)
        {
            var users = new List<User>();

            for (var i = 0; i < count; i++)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "test",
                    FirstName = "test",
                    LastName = "test",
                    Email = "test",
                    EmailConfirmed = true,
                    PasswordHash = "test",
                    PhoneNumber = "test",
                    PhoneNumberConfirmed = true,
                    ProfilePicture = "test"
                });
            }

            return users;
        }

        public static List<Review> GetTestReviews(int count = 5)
        {
            var reviews = new List<Review>();

            for (var i = 0; i < count; i++)
            {
                reviews.Add(new Review
                {
                    Id = Guid.NewGuid(),
                    Product = GetTestProducts(1)[0],
                    PublicationDate = DateTime.Now,
                    Rating = 5,
                    Text = "test",
                    Title = "test",
                    User = GetTestUsers(1)[0]
                });
            }

            return reviews;
        }
    }
}