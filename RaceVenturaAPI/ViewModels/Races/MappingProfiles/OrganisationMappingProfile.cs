using AutoMapper;
using RaceVenturaData.Models;
using RaceVenturaAPI.ViewModels.Admin;

namespace RaceVenturaAPI.ViewModels.Races.MappingProfiles
{
    public class OrganisationMappingProfile : Profile
    {
        public OrganisationMappingProfile()
        {
            CreateMap<Organisation, OrganisationViewModel>();
            CreateMap<OrganisationViewModel, Organisation>();
        }
    }
}
