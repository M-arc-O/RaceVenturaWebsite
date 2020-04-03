using Adventure4YouData.Models;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Race, RaceViewModel>();
            CreateMap<RaceViewModel, Race>();

            CreateMap<Race, RaceDetailViewModel>();
            CreateMap<RaceDetailViewModel, Race>();
        }
    }
}
