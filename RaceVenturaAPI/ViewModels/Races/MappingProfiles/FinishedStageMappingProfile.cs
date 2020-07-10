using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class FinishedStageMappingProfile : Profile
    {
        public FinishedStageMappingProfile()
        {
            CreateMap<FinishedStage, FinishedStageViewModel>();
            CreateMap<FinishedStageViewModel, FinishedStage>();
        }
    }
}
