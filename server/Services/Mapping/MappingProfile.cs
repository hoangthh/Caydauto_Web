using System.Drawing;
using AutoMapper;
using Microsoft.Identity.Client;

namespace server.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ProductMapping();
            UserMapping();
            CartMapping();
            CartItemMapping();
        }

        public void ProductMapping()
        {
            CreateMap<Product, ProductDetailGetDto>()
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
                    opt =>
                        opt.MapFrom(src =>
                            src.Comments.Any() ? src.Comments.Average(c => c.Rating) : 0
                        )
                )
                .ForMember(
                    dest => dest.IsNew,
                    opt => opt.MapFrom(src => (DateTime.Now - src.CreatedDate).TotalDays <= 7)
                );
            CreateMap<Product, ProductAllGetDto>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(src => src.Images.FirstOrDefault())
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
                    opt =>
                        opt.MapFrom(src =>
                            src.Comments.Any() ? src.Comments.Average(c => c.Rating) : 0
                        )
                );
            // Image -> ImageGetDto
            CreateMap<Image, ImageGetDto>();

            // Category -> CategoryGetDto
            CreateMap<Category, CategoryGetDto>();

            // Color -> ColorGetDto
            CreateMap<Color, ColorGetDto>()
                .ForMember(
                    dest => dest.HexCode,
                    opt =>
                        opt.MapFrom(src =>
                            "#" + string.Format(
                                "{0:x6}",
                                System.Drawing.Color.FromName(src.Name).ToArgb() & 0x00FFFFFF
                            )
                        )
                );

            CreateMap<ColorCreateDto, Color>()
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
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

        public void UserMapping()
        {
            // User -> UserGetDto
            CreateMap<User, UserGetDto>()
                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.FormatDate("dd/MM/yyyy"))
                )
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty)
                );

            // UserCreateDto -> User
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            // UserUpdateDto -> User
            CreateMap<UserPutDto, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());
        }

        public void CartMapping()
        {
            CreateMap<Cart, CartGetDto>()
                .ForMember(
                    dest => dest.CartItems,
                    opt => opt.MapFrom(src => src.CartItems ?? new List<CartItem>())
                )
                .ForMember(
                    dest => dest.TotalPrice,
                    opt =>
                        opt.MapFrom(src => src.CartItems.Sum(ci => ci.Quantity * ci.Product.Price))
                );
            CreateMap<CartItem, CartItemGetDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            CreateMap<Product, CartProdutGetDto>()
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.Images.FirstOrDefault()!.Url)
                );
        }

        public void CartItemMapping()
        {
            CreateMap<CartItemCreateDto, CartItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<CartItemGetDto, CartItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        public void OrderMapping()
        {
            CreateMap<Order, OrderGetDto>()
                .ForMember(
                    dest => dest.OrderItems,
                    opt => opt.MapFrom(src => src.OrderItems ?? new List<OrderItem>())
                )
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice))
                );
            CreateMap<OrderItem, OrderItemGetDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            CreateMap<Product, OrderItemProdutGetDto>()
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.Images.FirstOrDefault()!.Url)
                );
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<OrderItemCreateDto, OrderItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
