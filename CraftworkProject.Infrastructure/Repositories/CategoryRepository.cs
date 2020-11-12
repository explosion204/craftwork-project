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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Category> GetAllEntities()
        {
            var efCategories = _context.Categories.ToList();
            var categories = _mapper.Map<List<Category>>(efCategories);

            foreach (var category in categories)
            {
                category.Products =
                    _mapper.Map<List<Product>>(_context.Products.Where(x => x.CategoryId == category.Id));
            }

            return categories;
        }

        public Category GetEntity(Guid id)
        {
            var efCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            var category = _mapper.Map<Category>(efCategory);
            category.Products = _mapper.Map<List<Product>>(_context.Products.Where(x => x.CategoryId == category.Id));
            
            return category;
        }

        public void SaveEntity(Category entity)
        {
            var efCategory = _mapper.Map<EFCategory>(entity);

            _context.Entry(efCategory).State = efCategory.Id == default ? EntityState.Added : EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEntity(Guid id)
        {
            var efCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            //var efProducts = _context.Products.Where(x => x.CategoryId == id);
            
            if (efCategory != null)
            {
                //_context.Products.RemoveRange(efProducts);
                _context.Categories.Remove(efCategory);
            }

            _context.SaveChanges();
        }
    }
}