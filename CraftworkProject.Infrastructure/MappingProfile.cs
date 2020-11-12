using AutoMapper;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure.Models;

namespace CraftworkProject.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EFCategory, Category>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("Desc", opt => opt.MapFrom(src => src.Desc));
            
            CreateMap<Category, EFCategory>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("Desc", opt => opt.MapFrom(src => src.Desc));
            
            CreateMap<EFProduct, Product>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("ShortDesc", opt => opt.MapFrom(src => src.ShortDesc))
                .ForMember("Desc", opt => opt.MapFrom(src => src.Desc))
                .ForMember("Price", opt => opt.MapFrom(src => src.Price))
                .ForMember("InStock", opt => opt.MapFrom(src => src.InStock))
                .ForMember("ImagePath", opt => opt.MapFrom(src => src.ImagePath))
                .ForMember("Rating", opt => opt.MapFrom(src => src.Rating))
                .ForMember("RatesCount", opt => opt.MapFrom(src => src.RatesCount));

            CreateMap<Product, EFProduct>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("CategoryId", opt => opt.MapFrom(src => src.Category.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("ShortDesc", opt => opt.MapFrom(src => src.ShortDesc))
                .ForMember("Desc", opt => opt.MapFrom(src => src.Desc))
                .ForMember("Price", opt => opt.MapFrom(src => src.Price))
                .ForMember("InStock", opt => opt.MapFrom(src => src.InStock))
                .ForMember("ImagePath", opt => opt.MapFrom(src => src.ImagePath))
                .ForMember("Rating", opt => opt.MapFrom(src => src.Rating))
                .ForMember("RatesCount", opt => opt.MapFrom(src => src.RatesCount));

            CreateMap<PurchaseDetail, EFPurchaseDetail>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Amount", opt => opt.MapFrom(src => src.Amount));

            CreateMap<EFPurchaseDetail, PurchaseDetail>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Amount", opt => opt.MapFrom(src => src.Amount));

            CreateMap<PurchaseDetail, EFPurchaseDetail>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Amount", opt => opt.MapFrom(src => src.Amount))
                .ForMember("OrderId", opt => opt.MapFrom(src => src.Order.Id))
                .ForMember("ProductId", opt => opt.MapFrom(src => src.Product.Id));

            CreateMap<EFOrder, Order>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Processed", opt => opt.MapFrom(src => src.Processed))
                .ForMember("Finished", opt => opt.MapFrom(src => src.Finished));

            CreateMap<Order, EFOrder>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Processed", opt => opt.MapFrom(src => src.Processed))
                .ForMember("Finished", opt => opt.MapFrom(src => src.Finished))
                .ForMember("UserId", opt => opt.MapFrom(src => src.User.Id));

            CreateMap<EFReview, Review>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Title", opt => opt.MapFrom(src => src.Title))
                .ForMember("Text", opt => opt.MapFrom(src => src.Text))
                .ForMember("Rating", opt => opt.MapFrom(src => src.Rating))
                .ForMember("PublicationDate", opt => opt.MapFrom(src => src.PublicationDate));

            CreateMap<Review, EFReview>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Title", opt => opt.MapFrom(src => src.Title))
                .ForMember("Text", opt => opt.MapFrom(src => src.Text))
                .ForMember("Rating", opt => opt.MapFrom(src => src.Rating))
                .ForMember("UserId", opt => opt.MapFrom(src => src.User.Id))
                .ForMember("ProductId", opt => opt.MapFrom(src => src.Product.Id))
                .ForMember("PublicationDate", opt => opt.MapFrom(src => src.PublicationDate));

            CreateMap<EFUser, User>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Username", opt => opt.MapFrom(src => src.UserName))
                .ForMember("Email", opt => opt.MapFrom(src => src.Email))
                .ForMember("EmailConfirmed", opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember("PasswordHash", opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember("PhoneNumber", opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember("PhoneNumberConfirmed", opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember("ProfilePicture", opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<EFUserRole, UserRole>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name));
            
            CreateMap<UserRole, EFUserRole>()
                .ForMember("Id", opt => opt.MapFrom(src => src.Id))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name));
        }
    }
}