using AutoMapper;

namespace server.Services.Mapping.Profiles
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            // Product -> ProductGetDto
            CreateMap<Product, ProductGetDto>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images ?? new List<Image>())
                )
                .ForMember(
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => src.Categories ?? new List<Category>())
                )
                .ForMember(
                    dest => dest.Colors,
                    opt => opt.MapFrom(src => src.Colors ?? new List<Color>())
                )
                .ForMember(
                    dest => dest.AverageRating,
                    opt => opt.MapFrom(src => src.Comments.Any() ? src.Comments.Average(c => c.Rating) : 0)
                )
                .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => (DateTime.Now - src.CreatedDate).TotalDays <= 7));

            // Image -> ImageGetDto
            CreateMap<Image, ImageGetDto>();

            // Category -> CategoryGetDto
            CreateMap<Category, CategoryGetDto>();

            // Color -> ColorGetDto
            CreateMap<Color, ColorGetDto>();


            // ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.Colors, opt => opt.Ignore());

            // ProductPutDto -> Product
            CreateMap<ProductPutDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.Colors, opt => opt.Ignore());
        }
    }
}
