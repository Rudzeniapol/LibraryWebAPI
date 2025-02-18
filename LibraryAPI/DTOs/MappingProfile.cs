using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            
            CreateMap<CreateBookDTO, Book>();
            
            CreateMap<Author, AuthorDTO>();
        }
    }
}