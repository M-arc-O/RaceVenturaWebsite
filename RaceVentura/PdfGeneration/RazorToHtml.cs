using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RaceVentura.PdfGeneration
{
    public class RazorToHtml : IRazorToHtml
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for RazorToHtml
        /// </summary>
        /// <param name="viewEngine">View Engine to render Razor Pages</param>
        /// <param name="serviceProvider">Provides a Service Object</param>
        /// <param name="tempDataProvider">The Temporary Data Provider</param>
        /// <param name="logger">The logger instance for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="viewEngine"/> is null </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceProvider"/> is null </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tempDataProvider"/> is null </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null </exception>
        public RazorToHtml(IRazorViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider, ILogger<RazorToHtml> logger)
        {
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ///<inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="viewModel"/> is null </exception>
        public async Task<string> RenderRazorViewAsync<TViewModel>(string viewName, TViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            // add some fake contexts, we don't actually return content over http 
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var viewRender = _viewEngine.FindView(actionContext, viewName, false);

            if (viewRender == null || !viewRender.Success)
            {
                throw new InvalidOperationException($"Couldn't find view {viewName}");
            }

            var viewDictionary = new ViewDataDictionary<TViewModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = viewModel
            };

            var tempDataDictionary = new TempDataDictionary(httpContext, _tempDataProvider);

            using var stringWriter = new StringWriter();
            try
            {
                var viewContext = new ViewContext(actionContext, viewRender.View, viewDictionary,
                    tempDataDictionary, stringWriter, new HtmlHelperOptions());

                await viewRender.View.RenderAsync(viewContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in rendering {viewName}", ex);
                throw;
            }
            return stringWriter.ToString();
        }
    }
}
