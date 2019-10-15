using Adventure4You.Points;
using Adventure4You.Races;
using Adventure4You.Stages;
using Adventure4You.Teams;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure4You.Helpers
{
    public class BLHelper
    {
        public static void AddBLs(IServiceCollection services)
        {
            services.AddScoped<IAccountBL, AccountsBL>();
            services.AddScoped<IPointBL, PointBL>();
            services.AddScoped<IRaceBL, RaceBL>();
            services.AddScoped<IStageBL, StageBL>();
            services.AddScoped<ITeamBL, TeamBL>();
        }
    }
}
