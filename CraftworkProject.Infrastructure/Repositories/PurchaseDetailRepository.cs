using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure.Repositories
{
    public class PurchaseDetailRepository : IRepository<PurchaseDetail>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseDetailRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<PurchaseDetail> GetAllEntities()
        {
            var efPurchaseDetails = _context.PurchaseDetails.ToList();
            var purchaseDetails = _mapper.Map<List<PurchaseDetail>>(efPurchaseDetails);

            foreach (var pair in efPurchaseDetails.Zip(purchaseDetails,
                (efPurchaseDetail, purchaseDetail) => new
                    {EFPurchaseDetail = efPurchaseDetail, PurchaseDetail = purchaseDetail}))
            {
                pair.PurchaseDetail.Product =
                    _mapper.Map<Product>(
                        _context.Products.FirstOrDefault(x => x.Id == pair.EFPurchaseDetail.ProductId));
            }

            return purchaseDetails;
        }

        public PurchaseDetail GetEntity(Guid id)
        {
            var efPurchaseDetail = _context.PurchaseDetails.FirstOrDefault(x => x.Id == id);
            var purchaseDetail = _mapper.Map<PurchaseDetail>(efPurchaseDetail);
            purchaseDetail.Product =
                _mapper.Map<Product>(_context.Products.FirstOrDefault(x => x.Id == efPurchaseDetail.ProductId));

            return purchaseDetail;
        }

        public void SaveEntity(PurchaseDetail entity)
        {
            var efPurchaseDetail = _mapper.Map<EFPurchaseDetail>(entity);

            _context.Entry(efPurchaseDetail).State = efPurchaseDetail.Id == default ? EntityState.Added : EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEntity(Guid id)
        {
            var efPurchaseDetail = _context.PurchaseDetails.FirstOrDefault(x => x.Id == id);

            if (efPurchaseDetail != null)
            {
                _context.PurchaseDetails.Remove(efPurchaseDetail);
            }

            _context.SaveChanges();
        }
    }
}