using AutoMapper;
using LumaEventService.Models;
using LumaEventService.Models.DTO;

namespace LumaEventService.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Event, ReadEventDTO>()
                .ForMember(dest => dest.EventLocalDateStart, opt => opt.MapFrom(src => src.EventUtcDateStart.ToLocalTime()))
                .ForMember(dest => dest.EventLocalDateEnd, opt => opt.MapFrom(src => src.EventUtcDateEnd.ToLocalTime()));

            CreateMap<ReadEventDTO, Event>()
                .ForMember(dest => dest.EventUtcDateStart, opt => opt.MapFrom(src =>
                    (TimeZoneInfo.Local.BaseUtcOffset.Hours >= 0) ? src.EventLocalDateStart.AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours) : src.EventLocalDateStart.AddHours(-TimeZoneInfo.Local.BaseUtcOffset.Hours)))
                .ForMember(dest => dest.EventUtcDateEnd, opt => opt.MapFrom(src =>
                    (TimeZoneInfo.Local.BaseUtcOffset.Hours >= 0) ? src.EventLocalDateEnd.AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours) : src.EventLocalDateEnd.AddHours(-TimeZoneInfo.Local.BaseUtcOffset.Hours)));
        }
    }
}
