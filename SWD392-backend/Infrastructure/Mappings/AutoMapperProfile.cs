using AutoMapper;
using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<product, ProductResponse>();
            CreateMap<UpdateProductRequest, product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<AddProductRequest, product>();
            CreateMap<category, CategoryResponse>();
            CreateMap<supplier, SupplierrResponse>();
        }
    }
}
