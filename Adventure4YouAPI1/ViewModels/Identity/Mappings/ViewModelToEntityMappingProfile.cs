using AutoMapper;

using Adventure4YouAPI.Models.Identity;

namespace Adventure4YouAPI.ViewModels.Identity.Mappings
{
    public class ViewModelToEntityMappingProfile: Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
