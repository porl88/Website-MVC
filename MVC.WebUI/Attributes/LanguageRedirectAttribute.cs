namespace MVC.WebUI.Attributes
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class LanguageRedirectAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var currentLang = context.RouteData.Values["lang"] as string;

            if (request.HttpMethod == "GET" && (string.IsNullOrWhiteSpace(currentLang) || request.UrlReferrer == null || !request.UrlReferrer.Host.Equals(request.Url.Host, StringComparison.InvariantCultureIgnoreCase)))
            {
                var availableLanguages = new string[] { "en-gb", "fr-fr", "de-de" };
                var preferredLang = this.GetPreferredLanguage(request, availableLanguages, "en-gb");
                if (!preferredLang.Equals(currentLang, StringComparison.OrdinalIgnoreCase))
                {
                    var routes = context.RouteData.Values;
                    routes["lang"] = preferredLang;
                    // 301 redirects are cached aggressively by browsers
                    context.HttpContext.Response.CacheControl = "no-cache";
                    context.Result = new RedirectToRouteResult("DefaultLocalized", routes, true);
                }
            }
        }

        private string GetPreferredLanguage(HttpRequestBase request, string[] availableLanguages, string defaultLanguage)
        {
            var browserLanguages = request.UserLanguages;

            foreach (var item in browserLanguages)
            {
                var lang = item.Split(';').First();
                var langCode = availableLanguages.FirstOrDefault(x => x.Equals(lang, StringComparison.InvariantCultureIgnoreCase)) ??
                               availableLanguages.FirstOrDefault(x => x.Split('-').First().Equals(lang.Split('-').First(), StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrWhiteSpace(langCode))
                {
                    return langCode;
                }
            }

            return defaultLanguage;
        }
    }
}