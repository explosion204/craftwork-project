using System;
using System.Linq;
using Craftwork_Project.Domain.Models;

namespace Craftwork_Project.Domain.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetAllOrders();
        Order GetOrder(int id);
        void SaveOrder(Order category);
        void DeleteOrder(int id);
    }
}