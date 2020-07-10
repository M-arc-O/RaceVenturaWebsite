using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class RaceMappingProfile: Profile
    {
        public RaceMappingProfile()
        {
            CreateMap<Race, RaceViewModel>();
            CreateMap<RaceViewModel, Race>();

            CreateMap<Race, RaceDetailViewModel>();
            CreateMap<RaceDetailViewModel, Race>();
        }
    }
}
