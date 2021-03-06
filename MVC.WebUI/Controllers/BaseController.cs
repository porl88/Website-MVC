﻿namespace MVC.WebUI.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Exceptions;
    using Models.Shared;
    using Services.Culture;
    using Services.Culture.Transfer;

    public class BaseController : Controller
    {
        private readonly ILanguageService languageService;

        public BaseController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        [ChildActionOnly]
        public PartialViewResult Languages()
        {
            var model = this.PopulateLanguageViewModel();
            return this.PartialView("~/views/shared/_languages.cshtml", model);
        }

        [ChildActionOnly, HttpPost]
        public PartialViewResult Languages(string langCode)
        {
            var model = this.PopulateLanguageViewModel();
            var request = new SetPreferredLanguageRequest
            {
                LanguageCode = langCode
            };

            this.languageService.SetPreferredLanguage(request);
            return this.PartialView("~/views/shared/_languages.cshtml", model);
        }

        private LanguageListViewModel PopulateLanguageViewModel()
        {
            // BETTER TO SIMPLY HARD-CODE LANGUAGE LIST!!!
            var response = this.languageService.GetLanguages();
            var lang = this.RouteData.Values["lang"] as string;

            if (response.Status == StatusCode.OK && response.Languages.Any() && !string.IsNullOrWhiteSpace(lang))
            {
                var url = this.Request.Url.AbsolutePath;

                var model = new LanguageListViewModel
                {
                    Languages = response.Languages.Select(x => new LanguageDto
                    {
                        IsoCode = x.LanguageCode,
                        Name = x.Name,
                        Url = url.Replace($"/{lang}/", $"/{x.LanguageCode}/"),
                        IsSelected = x.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase)
                    }).ToList()
                };

                return model;
            }

            return null;
        }
    }
}