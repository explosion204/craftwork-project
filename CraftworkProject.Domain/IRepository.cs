using System;
using System.Linq;
using CraftworkProject.Domain;

namespace CraftworkProject.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAllEntities();
        T GetEntity(Guid id);
        void SaveEntity(T entity);
        void DeleteEntity(Guid id);
    }
}