// http://www.ryadel.com/en/setup-a-multi-language-website-using-asp-net-mvc/
namespace MVC.WebUI.Attributes
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;

    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // the default language is set in web.config, supported languages are set in RouteConfig.cs constraints for the language parameter
            var lang = filterContext.RouteData.Values["lang"] as string;

            if (lang != null)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }
                catch
                {
                    throw new NotSupportedException($"ERROR: Invalid language code '{lang}'.");
                }
            }
        }
    }
}