using Adventure4YouData.Models.Races;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Races.MappingProfiles
{
    public class TeamMappingProfile: Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamViewModel>();
            CreateMap<TeamViewModel, Team>();

            CreateMap<VisitedPoint, VisitedPointViewModel>();
            CreateMap<VisitedPointViewModel, VisitedPoint>();
        }
    }
}
