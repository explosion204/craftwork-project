using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure
{
    internal class OrderRepository : IRepository<Order>
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

                var efPurchaseDetails = _context.PurchaseDetails
                    .Where(x => x.OrderId == pair.EFOrder.Id)
                    .ToList();
                
                var purchaseDetails = new List<PurchaseDetail>();
                
                foreach (var efDetail in efPurchaseDetails)
                {
                    var detail = _mapper.Map<PurchaseDetail>(efDetail);
                    detail.Product = _mapper.Map<Product>(_context.Products.First(x => x.Id == efDetail.ProductId));
                    purchaseDetails.Add(detail);
                }

                pair.Order.PurchaseDetails = purchaseDetails;
            }

            return orders;
        }

        public Order GetEntity(Guid id)
        {
            var efOrder = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (efOrder == null)
                return null;
            
            var order = _mapper.Map<Order>(efOrder);
            order.User = _mapper.Map<User>(_userManager.Users.FirstOrDefault(x => x.Id == efOrder.UserId));
            var efPurchaseDetails = _context.PurchaseDetails
                .Where(x => x.OrderId == efOrder.Id)
                .ToList();
                
            var purchaseDetails = new List<PurchaseDetail>();
                
            foreach (var efDetail in efPurchaseDetails)
            {
                var detail = _mapper.Map<PurchaseDetail>(efDetail);
                detail.Product = _mapper.Map<Product>(_context.Products.First(x => x.Id == efDetail.ProductId));
                purchaseDetails.Add(detail);
            }

            order.PurchaseDetails = purchaseDetails;
            return order;
        }

        public Guid SaveEntity(Order entity)
        {
            var efOrder = _mapper.Map<EFOrder>(entity);
            

            if (efOrder.Id == default)
            {
                _context.Entry(efOrder).State = EntityState.Added;
            }
            else
            {
                var entry = _context.Orders.First(x => x.Id == efOrder.Id);
                _context.Entry(entry).State = EntityState.Detached;
                _context.Entry(efOrder).State = EntityState.Modified;
            }
            
            _context.SaveChanges();
            return efOrder.Id;
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