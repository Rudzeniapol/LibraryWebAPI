using AutoMapper;

namespace LibraryAPI.Application.DTOs.MappingProfiles;

public class ImageMappingProfile : Profile
{
    public ImageMappingProfile()
    {
        CreateMap<string, ImageUrlDTO>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => new ImageUrlDTO { ImageUrl = src }))
            .ReverseMap();
    }
}