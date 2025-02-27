using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.DTOs;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDTO, Book>().ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
    }
}