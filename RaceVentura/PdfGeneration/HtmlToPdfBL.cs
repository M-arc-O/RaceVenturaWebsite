using PuppeteerSharp;
using System.IO;
using System.Threading.Tasks;

namespace RaceVentura.PdfGeneration
{
    public class HtmlToPdfBL : IHtmlToPdfBL
    {
        public async Task<byte[]> ConvertHtmlToPdf(string html, string cssPath)
        {
            var pdfOptions = new PdfOptions();

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);
            await page.AddStyleTagAsync(new AddTagOptions { Path = cssPath });
            var stream = await page.PdfStreamAsync(pdfOptions);

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
