using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.DTOs;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<CreateBookDTO, Book>();
    }
}