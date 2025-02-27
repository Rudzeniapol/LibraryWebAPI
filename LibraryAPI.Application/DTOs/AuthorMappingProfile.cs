using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Domain.Models;

namespace LibraryAPI.Application.DTOs
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorDTO>();
        }
    }
}