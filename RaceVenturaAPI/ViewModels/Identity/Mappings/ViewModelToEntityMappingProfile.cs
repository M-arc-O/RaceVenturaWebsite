using AutoMapper;
using RaceVenturaData.Models.Identity;

namespace RaceVenturaAPI.ViewModels.Identity.Mappings
{
    public class ViewModelToEntityMappingProfile: Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
