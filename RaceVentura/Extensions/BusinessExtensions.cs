using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using RaceVentura.AppApi;
using RaceVentura.PdfGeneration;
using RaceVentura.Races;
using RaceVentura.Services;
using RaceVenturaData;
using RaceVenturaData.Models.Races;

namespace RaceVentura.Extensions
{
    public static class BusinessExtensions
    {        
        public static void AddBLs(this IServiceCollection services)
        {
            services.AddScoped<IRaceVenturaUnitOfWork, RaceVenturaUnitOfWork>();
            services.AddScoped<IAccountBL, AccountsBL>();
            services.AddScoped<IGenericCudBL<Point>, PointBL>();
            services.AddScoped<IGenericCrudBL<Race>, RaceBL>();
            services.AddScoped<IGenericCudBL<Stage>, StageBL>();
            services.AddScoped<IGenericCudBL<Team>, TeamBL>();
            services.AddScoped<IGenericCudBL<VisitedPoint>, VisitedPointBL>();
            services.AddScoped<IResultsBL, ResultsBL>();
            services.AddScoped<IRaceAccessBL, RaceAccessBL>();

            services.AddScoped<IAppApiBL, AppApiBL>();

            services.AddScoped<IHtmlToPdfBL, HtmlToPdfBL>();
            services.AddScoped<IRazorToHtml, RazorToHtml>();

            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
