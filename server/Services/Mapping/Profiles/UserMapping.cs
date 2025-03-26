using AutoMapper;

namespace server.Services.Mapping.Profiles
{
    public class UserMapping : Profile
    {
        public UserMapping()
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
    }
}
