using System;
using System.Linq;
using CraftworkProject.Domain;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private ApplicationDbContext context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IQueryable<T> GetAllEntities()
        {
            return entities.AsQueryable();
        }

        public T GetEntity(Guid id)
        {
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public void SaveEntity(T entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteEntity(Guid id)
        {
            T entity = entities.FirstOrDefault(x => x.Id == id);
            
            if (entity != null)
            {
                entities.Remove(entity);
            }
            context.SaveChanges();
        }
    }
}