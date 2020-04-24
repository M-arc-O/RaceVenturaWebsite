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
            services.AddScoped<IGenericCudBL<Point>, PointBL>();
            services.AddScoped<IGenericCrudBL<Race>, RaceBL>();
            services.AddScoped<IGenericCudBL<Stage>, StageBL>();
            services.AddScoped<IGenericCudBL<Team>, TeamBL>();
            services.AddScoped<IGenericCudBL<VisitedPoint>, VisitedPointBL>();
            services.AddScoped<IResultsBL, ResultsBL>();
        }
    }
}
