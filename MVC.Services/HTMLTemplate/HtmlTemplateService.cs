namespace MVC.Services.HTMLTemplate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Core.Exceptions;
    using Transfer;

    public class HtmlTemplateService : IHtmlTemplateService
    {
        private readonly IExceptionHandler exceptionHandler;

        public HtmlTemplateService(IExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }

        public GetHtmlResponse GetHtml(GetHtmlRequest request)
        {
            var response = new GetHtmlResponse();

            try
            {
                var html = this.GetHtmlFromFile(request.LayoutFilePath);
                html = this.AddInnerHtml(html, request.InnerHtml);
                html = this.AddMappings(html, request.Mappings);
                html = this.RemoveUnusedMarkers(html);
                response.Status = StatusCode.OK;
                response.Html = html;
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
            }

            return response;
        }

        private string GetHtmlFromFile(string htmlFilePath)
        {
            string html = string.Empty;

            if (File.Exists(htmlFilePath))
            {
                html = File.ReadAllText(htmlFilePath);
            }

            return html;
        }

        private string AddInnerHtml(string html, Dictionary<string, string> innerHtml)
        {
            if (innerHtml != null)
            {
                foreach (var path in innerHtml)
                {
                    var htmlFragment = this.GetHtmlFromFile(path.Value);
                    html = html.Replace($"##{path.Key}##", htmlFragment);
                }
            }

            return html;
        }

        private string AddMappings(string html, Dictionary<string, string> mappings)
        {
            if (mappings != null && !string.IsNullOrWhiteSpace(html))
            {
                foreach (var item in mappings)
                {
                    if (item.Value != null)
                    {
                        html = html.Replace($"##{item.Key}##", item.Value);
                    }
                }
            }

            return html;
        }

        private string RemoveUnusedMarkers(string html)
        {
            html = Regex.Replace(html, @"##[A-Z]*##", string.Empty);
            return html;
        }
    }
}
