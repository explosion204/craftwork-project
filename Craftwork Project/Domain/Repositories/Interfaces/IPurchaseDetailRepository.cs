using System;
using System.Linq;
using Craftwork_Project.Domain.Models;

namespace Craftwork_Project.Domain.Repositories.Interfaces
{
    public interface IPurchaseDetailRepository
    {
        IQueryable<PurchaseDetail> GetAllPurchaseDetails();
        PurchaseDetail GetPurchaseDetail(Guid id);
        void SavePurchaseDetail(PurchaseDetail purchaseDetail);
        void DeletePurchaseDetail(Guid id);
    }
}