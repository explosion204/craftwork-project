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
    public class ReviewRepository : IRepository<Review>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<EFUser> _userManager;

        public ReviewRepository(ApplicationDbContext context, IMapper mapper, UserManager<EFUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public List<Review> GetAllEntities()
        {
            var efReviews = _context.Reviews.ToList();
            var reviews = _mapper.Map<List<Review>>(efReviews);

            foreach (var pair in efReviews.Zip(reviews,
                (efReview, review) => new
                    {EFReview = efReview, Review = review}))
            {
                pair.Review.User =
                    _mapper.Map<User>(_userManager.Users.FirstOrDefault(x => x.Id == pair.EFReview.UserId));
                pair.Review.Product =
                    _mapper.Map<Product>(_context.Products.FirstOrDefault(x => x.Id == pair.EFReview.ProductId));
            }
            
            return reviews;
        }

        public Review GetEntity(Guid id)
        {
            var efReview = _context.Reviews.FirstOrDefault(x => x.Id == id);
            var review = _mapper.Map<Review>(efReview);
            review.User = _mapper.Map<User>(_userManager.Users.FirstOrDefault(x => x.Id == efReview.UserId));
            review.Product = _mapper.Map<Product>(_context.Products.FirstOrDefault(x => x.Id == efReview.ProductId));
           
            return review;
        }

        public void SaveEntity(Review entity)
        {
            EFReview efReview;
            var efProduct = _mapper.Map<EFProduct>(entity.Product);
            var entry = _context.Products.First(x => x.Id == efProduct.Id);
            _context.Entry(entry).State = EntityState.Detached;

            if (entity.Id == default)
            {
                efReview = _mapper.Map<EFReview>(entity);
                efProduct.Rating = (efProduct.Rating * efProduct.RatesCount + efReview.Rating) / (efProduct.RatesCount + 1);
                efProduct.RatesCount++;
                
                _context.Entry(efReview).State = EntityState.Added;
            }
            else
            {
                efReview = _context.Reviews.FirstOrDefault(x => x.Id == entity.Id);
                efProduct.Rating = (efProduct.Rating * efProduct.RatesCount - efReview!.Rating + entity.Rating) /
                                   efProduct.RatesCount;

                efReview.Title = entity.Title;
                efReview.Text = entity.Text;
                efReview.Rating = entity.Rating;
                
                _context.Entry(efReview).State = EntityState.Modified;
            }

            
            _context.Entry(efProduct).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEntity(Guid id)
        {
            var efReview = _context.Reviews.FirstOrDefault(x => x.Id == id);
            var efProduct = _context.Products.FirstOrDefault(x => x.Id == efReview.ProductId);

            if (efReview != null && efProduct != null)
            {
                efProduct.Rating = efProduct.RatesCount - 1 != 0
                    ? (efProduct.Rating * efProduct.RatesCount - efReview.Rating) / (efProduct.RatesCount - 1)
                    : 0;
                efProduct.RatesCount--;
                
                _context.Reviews.Remove(efReview);
            }

            _context.SaveChanges();
        }
    }
}