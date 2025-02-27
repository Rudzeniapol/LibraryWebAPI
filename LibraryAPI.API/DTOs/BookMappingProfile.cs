using AutoMapper;
using LibraryAPI.Domain.Models;
using LibraryAPI.API.DTOs;

namespace LibraryAPI.API.DTOs;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDTO, Book>().ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
    }
}