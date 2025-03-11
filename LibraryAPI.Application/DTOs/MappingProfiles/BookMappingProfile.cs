using AutoMapper;
using LibraryAPI.Domain.Models;
using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.DTOs.MappingProfiles;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDTO, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BorrowedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ReturnBy, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ReverseMap();
    }
}