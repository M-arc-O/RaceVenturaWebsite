using RaceVentura.Models.Results;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
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
