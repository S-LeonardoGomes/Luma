using AutoMapper;
using EventService.Models;

namespace EventService.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.UserEventDateStart, opt => opt.MapFrom(src => src.UtcEventDateStart.ToLocalTime()))
                .ForMember(dest => dest.UserEventDateEnd, opt => opt.MapFrom(src => src.UtcEventDateEnd.ToLocalTime()));

            CreateMap<EventDTO, Event>()
                .ForMember(dest => dest.UtcEventDateStart, opt => opt.MapFrom(src => src.UserEventDateStart.ToUniversalTime()))
                .ForMember(dest => dest.UtcEventDateEnd, opt => opt.MapFrom(src => src.UserEventDateEnd.ToUniversalTime()));                
        }
    }
}
