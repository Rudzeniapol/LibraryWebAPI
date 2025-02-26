using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorDTO>();
        }
    }
}