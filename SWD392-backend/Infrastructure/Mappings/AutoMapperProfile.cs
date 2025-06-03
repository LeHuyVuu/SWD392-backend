using AutoMapper;
using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;
using SWD392_backend.Models.ElasticDocs;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<product, ProductResponse>()               
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.product_images.Count(i => i.IsMain) > 0 ? src.product_images.FirstOrDefault(i => i.IsMain).ProductImageUrl : "https://cdn.kusl.io.vn/dien-tu-cong-nghe/macbook_air_m2_1.webp"));
            CreateMap<UpdateProductRequest, product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<product, ProductDetailResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.product_images));
            CreateMap<AddProductRequest, product>();
            CreateMap<product, ProductElasticDoc>();
            CreateMap<category, CategoryResponse>();
            CreateMap<supplier, SupplierrResponse>();
            CreateMap<ProductImageRequest, product_image>();
            CreateMap<product_image, ProductImageResponse>();
            CreateMap<order, OrderResponse>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.orders_details));

            CreateMap<orders_detail, OrderDetailResponse>();

        }
    }
}