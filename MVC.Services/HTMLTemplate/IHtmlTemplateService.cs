namespace MVC.Services.HTMLTemplate
{
    using Transfer;

    public interface IHtmlTemplateService
    {
        GetHtmlResponse GetHtml(GetHtmlRequest request);
    }
}
