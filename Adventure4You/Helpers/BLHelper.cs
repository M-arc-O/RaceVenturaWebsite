using Adventure4You.Races;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure4You.Helpers
{
    public class BLHelper
    {
        public static void AddBLs(IServiceCollection services)
        {
            services.AddScoped<IAccountBL, AccountsBL>();
            services.AddScoped<IGenericBL<Point>, PointBL>();
            services.AddScoped<IRaceBL, RaceBL>();
            services.AddScoped<IGenericBL<Stage>, StageBL>();
            services.AddScoped<IGenericBL<Team>, TeamBL>();
            services.AddScoped<IGenericBL<VisitedPoint>, VisitedPointBL>();
            services.AddScoped<IResultsBL, ResultsBL>();
        }
    }
}
