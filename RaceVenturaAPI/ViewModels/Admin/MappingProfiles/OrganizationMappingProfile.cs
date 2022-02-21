using AutoMapper;
using RaceVenturaData.Models.Organization;

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
