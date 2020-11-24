using System.Threading.Tasks;

namespace RaceVentura.PdfGeneration
{
    public interface IHtmlToPdfBL
    {
        Task<byte[]> ConvertHtmlToPdf(string html, string cssPath);
    }
}