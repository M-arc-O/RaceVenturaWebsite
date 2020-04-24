using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
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
