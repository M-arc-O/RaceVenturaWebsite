using System.Threading.Tasks;

namespace RaceVentura.PdfGeneration
{
    public interface IRazorToHtml
    {
        Task<string> RenderRazorViewAsync<TViewModel>(string viewName, TViewModel viewModel);
    }
}