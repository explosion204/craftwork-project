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
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Product> GetAllEntities()
        {
            var efProducts = _context.Products.ToList();
            var products = _mapper.Map<List<Product>>(efProducts);

            foreach (var pair in efProducts.Zip(products, 
                (efProduct, product) => new { EFProduct = efProduct, Product = product }))
            {
                pair.Product.Category =
                    _mapper.Map<Category>(_context.Categories.FirstOrDefault(x => x.Id == pair.EFProduct.CategoryId));
            }

            return products;
        }

        public Product GetEntity(Guid id)
        {
            var efProduct = _context.Products.FirstOrDefault(x => x.Id == id);

            if (efProduct == null)
                return null;
            
            var product = _mapper.Map<Product>(efProduct);
            product.Category =
                _mapper.Map<Category>(_context.Categories.FirstOrDefault(x => x.Id == efProduct.CategoryId));

            return product;
        }

        public Guid SaveEntity(Product entity)
        {
            var efProduct = _mapper.Map<EFProduct>(entity);

            _context.Entry(efProduct).State = efProduct.Id == default ? EntityState.Added : EntityState.Modified;
            _context.SaveChanges();

            return efProduct.Id;
        }

        public void DeleteEntity(Guid id)
        {
            var efProduct = _context.Products.FirstOrDefault(x => x.Id == id);

            if (efProduct != null)
            {
                _context.Products.Remove(efProduct);
            }

            _context.SaveChanges();
        }
    }
}