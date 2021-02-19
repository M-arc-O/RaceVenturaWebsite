using RaceVentura.AppApi;
using RaceVentura.Races;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.DependencyInjection;
using RaceVentura.PdfGeneration;
using Microsoft.AspNetCore.Identity.UI.Services;
using RaceVentura.Services;

namespace RaceVentura.Helpers
{
    public class BLHelper
    {
        public static void AddBLs(IServiceCollection services)
        {
            services.AddScoped<IRaceVenturaUnitOfWork, RaceVenturaUnitOfWork>();
            services.AddScoped<IAccountBL, AccountsBL>();
            services.AddScoped<IGenericCudBL<Point>, PointBL>();
            services.AddScoped<IGenericCrudBL<Race>, RaceBL>();
            services.AddScoped<IGenericCudBL<Stage>, StageBL>();
            services.AddScoped<IGenericCudBL<Team>, TeamBL>();
            services.AddScoped<IGenericCudBL<VisitedPoint>, VisitedPointBL>();
            services.AddScoped<IResultsBL, ResultsBL>();
            services.AddScoped<IAppApiBL, AppApiBL>();
            services.AddScoped<IHtmlToPdfBL, HtmlToPdfBL>();
            services.AddScoped<IRazorToHtml, RazorToHtml>();

            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
