using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
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
