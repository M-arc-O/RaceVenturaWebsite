using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
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
