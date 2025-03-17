using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.Application.DTOs.MappingProfiles
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<AuthorDTO, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();  
            CreateMap<ChangeAuthorDTO, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}