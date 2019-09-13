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

            CreateMap<PointDetailViewModel, Point>();
            CreateMap<Point, PointDetailViewModel>();
        }
    }
}
