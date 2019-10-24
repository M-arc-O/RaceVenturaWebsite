using Adventure4You.Models.Teams;
using AutoMapper;

namespace Adventure4YouAPI.ViewModels.Teams
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamDetailViewModel>();
            CreateMap<TeamDetailViewModel, Team>();
        }
    }
}
