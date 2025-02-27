using AutoMapper;
using LibraryAPI.API.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.API
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorDTO>();
        }
    }
}