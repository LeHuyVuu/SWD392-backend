using AutoMapper;
using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<product, ProductResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.created_at))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.StockInQuantity, opt => opt.MapFrom(src => src.stock_in_quantity))
                .ForMember(dest => dest.RatingAverage, opt => opt.MapFrom(src => src.rating_average))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.sku))
                .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src => src.discount_price))
                .ForMember(dest => dest.DiscountPercent, opt => opt.MapFrom(src => src.discount_percent))
                .ForMember(dest => dest.SoldQuantity, opt => opt.MapFrom(src => src.sold_quantity))
                .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.available_quantity))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.is_active))
                .ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.is_sale))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.slug))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.product_images.Count(i => i.is_main) > 0 ? src.product_images.FirstOrDefault(i => i.is_main).product_image_url : "https://via.placeholder.com/150"));
            CreateMap<UpdateProductRequest, product>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.stock_in_quantity, opt => opt.MapFrom(src => src.StockInQuantity))
                .ForMember(dest => dest.discount_percent, opt => opt.MapFrom(src => src.DiscountPercent))
                .ForMember(dest => dest.is_sale, opt => opt.MapFrom(src => src.IsSale))
                .ForMember(dest => dest.sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.categories_id, opt => opt.MapFrom(src => src.CategoriesId));
            CreateMap<category, CategoryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.slug))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.image_url));
            CreateMap<supplier, SupplierrResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.slug))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.image_url));
        }
    }
}
