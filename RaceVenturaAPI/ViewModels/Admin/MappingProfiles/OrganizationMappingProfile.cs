using AutoMapper;
using RaceVenturaData.Models;
using RaceVenturaAPI.ViewModels.Admin;

namespace RaceVenturaAPI.ViewModels.Admin.MappingProfiles
{
    public class OrganizationMappingProfile : Profile
    {
        public OrganizationMappingProfile()
        {
            CreateMap<Organization, OrganizationViewModel>();
            CreateMap<OrganizationViewModel, Organization>();
        }
    }
}
