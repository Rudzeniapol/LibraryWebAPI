using AutoMapper;
using LibraryAPI.Domain.Models;
using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.DTOs;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDTO, Book>().ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
    }
}