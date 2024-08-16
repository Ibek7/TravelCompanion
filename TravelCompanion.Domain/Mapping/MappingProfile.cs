using AutoMapper;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Models;

namespace TravelCompanion.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<TripChat, TripChatDto>().ReverseMap();
            CreateMap<TripChat, TripChatInfoDto>().ReverseMap();
            CreateMap<TripEvent, TripEventDto>().ReverseMap();
        }
    }
}
