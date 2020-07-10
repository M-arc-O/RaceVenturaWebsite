using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class StageMappingProfile: Profile
    {
        public StageMappingProfile()
        {
            CreateMap<Stage, StageViewModel>();
            CreateMap<StageViewModel, Stage>();
        }
    }
}
