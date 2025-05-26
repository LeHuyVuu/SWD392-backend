using AutoMapper;
using SWD392_backend.Entities;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<product, ProductResponse>();
        }
    }
}
