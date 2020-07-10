using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
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
