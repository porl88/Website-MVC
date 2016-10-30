namespace MVC.Services.HTMLTemplate.Transfer
{
    using System.Collections.Generic;

    public class GetHtmlRequest
    {
        public GetHtmlRequest(string layoutFilePath)
        {
            this.InnerHtml = new Dictionary<string, string>();
            this.Mappings = new Dictionary<string, string>();
            this.LayoutFilePath = layoutFilePath;
        }

        public string LayoutFilePath { get; private set; }

        public Dictionary<string, string> InnerHtml { get; set; }

        public Dictionary<string, string> Mappings { get; set; }

        public void AddInnerHtml(string key, string filePath)
        {
            this.InnerHtml.Add(key, filePath);
        }

        public void AddMapping(string key, string value)
        {
            this.Mappings.Add(key, value);
        }
    }
}
