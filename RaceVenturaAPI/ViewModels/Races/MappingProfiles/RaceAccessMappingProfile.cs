using AutoMapper;
using RaceVentura.Models.RaceAccess;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class RaceAccessMappingProfile : Profile
    {
        public RaceAccessMappingProfile()
        {
            CreateMap<RaceAccess, RaceAccessViewModel>()
                .ForMember(dest => dest.RaceId, src => src.MapFrom(x => x.RaceId))
                .ForMember(dest => dest.AccessLevel, src => src.MapFrom(x => x.RaceAccessLevel))
                .ForMember(dest => dest.UserEmail, src => src.MapFrom(x => x.Email));
            CreateMap<RaceAccessViewModel, RaceAccess>()
                .ForMember(dest => dest.RaceId, src => src.MapFrom(x => x.RaceId))
                .ForMember(dest => dest.RaceAccessLevel, src => src.MapFrom(x => x.AccessLevel))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.UserEmail));
        }
    }
}
