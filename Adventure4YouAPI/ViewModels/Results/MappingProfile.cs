using Adventure4You.Models.Results;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Results
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeamResult, TeamResultViewModel>();
            CreateMap<TeamResultViewModel, TeamResult>();

            CreateMap<StageResult, StageResultViewModel>();
            CreateMap<StageResultViewModel, StageResult>();

            CreateMap<PointResult, PointResultViewModel>();
            CreateMap<PointResultViewModel, PointResult>();
        }
    }
}
