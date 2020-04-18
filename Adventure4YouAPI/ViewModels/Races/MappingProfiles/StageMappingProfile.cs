using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
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
