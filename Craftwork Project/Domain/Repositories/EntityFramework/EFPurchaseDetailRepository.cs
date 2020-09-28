using System;
using System.Linq;
using Craftwork_Project.Domain.Models;
using Craftwork_Project.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Craftwork_Project.Domain.Repositories.EntityFramework
{
    public class EFPurchaseDetailRepository : IPurchaseDetailRepository
    {
        private ApplicationDbContext context;

        public EFPurchaseDetailRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<PurchaseDetail> GetAllPurchaseDetails()
        {
            return context.PurchaseDetails;
        }

        public PurchaseDetail GetPurchaseDetail(Guid id)
        {
            return context.PurchaseDetails.FirstOrDefault(x => x.Id == id);
        }

        public void SavePurchaseDetail(PurchaseDetail purchaseDetail)
        {
            if (purchaseDetail.Id == default)
            {
                context.Entry(purchaseDetail).State = EntityState.Added;
            }
            else
            {
                context.Entry(purchaseDetail).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeletePurchaseDetail(Guid id)
        {
            context.PurchaseDetails.Remove(new PurchaseDetail() { Id = id } );
            context.SaveChanges();
        }
    }
}