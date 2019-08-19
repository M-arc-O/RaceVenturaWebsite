using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
