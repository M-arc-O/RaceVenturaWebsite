using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure4YouAPI.ViewModels
{
    public static class AddMappers
    {

        public static void AddAutoMapper(IServiceCollection services)
        {   // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Identity.Mappings.ViewModelToEntityMappingProfile());
                mc.AddProfile(new Points.MappingProfile());
                mc.AddProfile(new Stages.MappingProfile());
                mc.AddProfile(new Races.MappingProfile());
                mc.AddProfile(new Teams.MappingProfile());
                mc.AddProfile(new Results.MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
