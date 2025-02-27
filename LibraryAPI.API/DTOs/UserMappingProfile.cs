using AutoMapper;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.API.DTOs;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDTO, User>();
        CreateMap<LoginUserDTO, User>();
    }
}