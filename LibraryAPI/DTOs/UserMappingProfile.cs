using AutoMapper;
using LibraryAPI.Models;

namespace LibraryAPI.DTOs;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDTO, User>();
        CreateMap<LoginUserDTO, User>();
    }
}