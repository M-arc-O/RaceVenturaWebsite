using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RaceVenturaAPI.ViewModels.Admin.MappingProfiles;
using RaceVenturaAPI.ViewModels.Identity.Mappings;
using RaceVenturaAPI.ViewModels.Races.MappingProfiles;

namespace RaceVenturaAPI.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {   // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ViewModelToEntityMappingProfile());
                mc.AddProfile(new PointMappingProfile());
                mc.AddProfile(new StageMappingProfile());
                mc.AddProfile(new RaceMappingProfile());
                mc.AddProfile(new RaceAccessMappingProfile());
                mc.AddProfile(new TeamMappingProfile());
                mc.AddProfile(new VisitedPointMappingProfile());
                mc.AddProfile(new FinishedStageMappingProfile());
                mc.AddProfile(new ResultMappingProfile());
                mc.AddProfile(new OrganizationMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
