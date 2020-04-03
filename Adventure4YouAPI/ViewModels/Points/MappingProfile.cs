using Adventure4YouData.Models.Points;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Points
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PointDetailViewModel, Point>();
            CreateMap<Point, PointDetailViewModel>();
        }
    }
}
