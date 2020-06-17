using Adventure4You.Models.Results;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
{
    public class ResultMappingProfile : Profile
    {
        public ResultMappingProfile()
        {
            CreateMap<TeamResult, TeamResultViewModel>();

            CreateMap<StageResult, StageResultViewModel>();

            CreateMap<PointResult, PointResultViewModel>();
        }
    }
}
