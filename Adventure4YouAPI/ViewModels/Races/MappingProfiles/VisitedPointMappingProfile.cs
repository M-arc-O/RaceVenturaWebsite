using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
{
    public class VisitedPointMappingProfile : Profile
    {
        public VisitedPointMappingProfile()
        {
            CreateMap<VisitedPoint, VisitedPointViewModel>();
            CreateMap<VisitedPointViewModel, VisitedPoint>();
        }
    }
}
