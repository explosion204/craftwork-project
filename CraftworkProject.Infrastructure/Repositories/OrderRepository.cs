using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<EFUser> _userManager;

        public OrderRepository(ApplicationDbContext context, IMapper mapper, UserManager<EFUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public List<Order> GetAllEntities()
        {
            var efOrders = _context.Orders.ToList();
            var orders = _mapper.Map<List<Order>>(efOrders);

            foreach (var pair in efOrders.Zip(orders,
                (efOrder, order) => new { EFOrder = efOrder, Order = order }))
            {
                pair.Order.User =
                    _mapper.Map<User>(_userManager.Users.FirstOrDefault(x => x.Id == pair.EFOrder.UserId));
                pair.Order.PurchaseDetails =
                    _mapper.Map<List<PurchaseDetail>>(
                        _context.PurchaseDetails.Where(x => x.OrderId == pair.EFOrder.Id));
            }

            return orders;
        }

        public Order GetEntity(Guid id)
        {
            var efOrder = _context.Orders.FirstOrDefault(x => x.Id == id);
            var order = _mapper.Map<Order>(efOrder);
            order.User = _mapper.Map<User>(_userManager.Users.FirstOrDefault(x => x.Id == efOrder.UserId));
            order.PurchaseDetails =
                _mapper.Map<List<PurchaseDetail>>(_context.PurchaseDetails.Where(x => x.OrderId == efOrder.Id));

            return order;
        }

        public void SaveEntity(Order entity)
        {
            var efOrder = _mapper.Map<EFOrder>(entity);

            _context.Entry(efOrder).State = efOrder.Id == default ? EntityState.Added : EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEntity(Guid id)
        {
            var efOrder = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (efOrder != null)
            {
                _context.Orders.Remove(efOrder);
            }

            _context.SaveChanges();
        }
    }
}