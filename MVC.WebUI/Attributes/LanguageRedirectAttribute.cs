namespace MVC.WebUI.Attributes
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Ninject;
    using Services.Culture;
    using Services.Culture.Transfer;

    public class LanguageRedirectAttribute : ActionFilterAttribute
    {
        [Inject]
        public ILanguageService languageService;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var lang = context.RouteData.Values["lang"] as string;

            if (lang == null)
            {
                var request = new GetPreferredLanguageRequest
                {
                    AvailableLanguages = new string[] { "en-gb", "fr-fr", "de-de" }
                };

                var preferredLang = this.languageService.GetPreferredLanguage(request);

                var routes = new RouteValueDictionary
                {
                    { "lang", preferredLang },
                    { "controller", context.RouteData.Values["controller"] },
                    { "action", context.RouteData.Values["action"] },
                    { "id", context.RouteData.Values["id"] },
                };

                context.Result = new RedirectToRouteResult("DefaultLocalized", routes, true);
            }
        }

        //private string GetPreferredLanguage(HttpRequestBase request, string[] availableLanguages, string defaultLanguage)
        //{
        //    var browserLanguages = request.UserLanguages;

        //    foreach (var item in browserLanguages)
        //    {
        //        var lang = item.Split(';').First();
        //        var langCode = availableLanguages.FirstOrDefault(x => x.Equals(lang, StringComparison.InvariantCultureIgnoreCase)) ??
        //                       availableLanguages.FirstOrDefault(x => x.Split('-').First().Equals(lang.Split('-').First(), StringComparison.InvariantCultureIgnoreCase));

        //        if (!string.IsNullOrWhiteSpace(langCode))
        //        {
        //            return langCode;
        //        }
        //    }

        //    return defaultLanguage;
        //}
    }
}