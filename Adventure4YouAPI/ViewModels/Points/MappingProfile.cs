using Adventure4You.Models.Points;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Points
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Point, PointViewModel>();
            CreateMap<PointViewModel, Point>();

            CreateMap<AddPointViewModel, Point>();
            CreateMap<Point, AddPointViewModel>();
        }
    }
}
