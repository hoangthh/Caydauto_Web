using AutoMapper;

namespace server.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product -> ProductGetDto
            CreateMap<Product, ProductGetDto>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images ?? new List<Image>())
                );

            // Image -> ImageGetDto
            CreateMap<Image, ImageGetDto>();

            // ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // ProductPutDto -> Product
            CreateMap<ProductPutDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }

        
    }
}
