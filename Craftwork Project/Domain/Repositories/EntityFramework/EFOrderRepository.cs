using System;
using System.Linq;
using Craftwork_Project.Domain.Models;
using Craftwork_Project.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Craftwork_Project.Domain.Repositories.EntityFramework
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Order> GetAllOrders()
        {
            return context.Orders;
        }

        public Order GetOrder(int id)
        {
            return context.Orders.FirstOrDefault(x => x.Id == id);
        }

        public void SaveOrder(Order order)
        {
            if (order.Id == default)
            {
                context.Entry(order).State = EntityState.Added;
            }
            else
            {
                context.Entry(order).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            context.Orders.Remove(new Order() { Id = id } );
            context.SaveChanges();
        }
    }
}