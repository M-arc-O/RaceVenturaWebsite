using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class PointMappingProfile : Profile
    {
        public PointMappingProfile()
        {
            CreateMap<PointViewModel, Point>();
            CreateMap<Point, PointViewModel>();
        }
    }
}
