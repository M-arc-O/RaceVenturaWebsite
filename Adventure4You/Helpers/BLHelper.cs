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
            services.AddScoped<RaceBL>();
        }
    }
}
