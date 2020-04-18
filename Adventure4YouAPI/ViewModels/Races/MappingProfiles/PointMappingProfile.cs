using AutoMapper;
using System.Drawing;

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
