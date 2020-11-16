using System;
using System.Collections.Generic;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        List<T> GetAllEntities();
        T GetEntity(Guid id);
        Guid SaveEntity(T entity);
        void DeleteEntity(Guid id);
    }
}