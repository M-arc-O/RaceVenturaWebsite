using RaceVenturaData.Models.Races;
using AutoMapper;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class TeamMappingProfile: Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamViewModel>();
            CreateMap<TeamViewModel, Team>();
        }
    }
}
