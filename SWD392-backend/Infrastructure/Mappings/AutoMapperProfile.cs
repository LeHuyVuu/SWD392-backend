using AutoMapper;
using SWD392_backend.Entities;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<product, ProductResponse>();
            CreateMap<category, CategoryResponse>();
        }
    }
}
