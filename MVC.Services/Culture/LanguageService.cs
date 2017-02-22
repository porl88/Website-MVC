namespace MVC.Services.Culture
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Core.Data;
    using Core.Exceptions;
    using Storage;
    using Transfer;

    public class LanguageService : ILanguageService
    {
        private readonly HttpContextBase context;
        private readonly IExceptionHandler exceptionHandler;
        private readonly ILocalStorageService storageService;
        private readonly IUnitOfWork unitOfWork;
        private const string defaultLanguage = "en-gb";
        private const string key = "lang";

        public LanguageService(
            HttpContextBase context,
            IExceptionHandler exceptionHandler,
            ILocalStorageService storageService,
            IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.exceptionHandler = exceptionHandler;
            this.storageService = storageService;
            this.unitOfWork = unitOfWork;
        }

        public GetLanguageResponse GetLanguage(GetLanguageRequest request)
        {
            var response = new GetLanguageResponse();
            var isValidFormat = Regex.IsMatch(request.LanguageCode, "[a-z]{2}(-[a-z]{2})?", RegexOptions.IgnoreCase);

            if (!isValidFormat)
            {
                response.Message = "The language code is in the wrong format.";
                return response;
            }

            try
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                var culture = cultures.FirstOrDefault(x => x.Name.Equals(request.LanguageCode, StringComparison.InvariantCultureIgnoreCase));
                var language = new LanguageDto();

                if (culture != null)
                {
                    language.IsoCode = culture.IetfLanguageTag;
                    language.Name = culture.EnglishName;
                    language.NativeName = culture.NativeName;
                }

                response.Language = language;
                response.Success = true;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Common.InternalServerError;
            }

            return response;
        }

        public GetLanguagesResponse GetAllLanguages()
        {
            var response = new GetLanguagesResponse();

            try
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
                var languages = cultures
                .OrderBy(x => x.NativeName)
                .Select(x => new LanguageDto
                {
                    IsoCode = x.TwoLetterISOLanguageName,
                    Name = x.EnglishName,
                    NativeName = x.NativeName
                }).ToList();

                response.Languages = languages;
                response.Success = true;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Common.InternalServerError;
            }

            return response;
        }

        public GetLanguagesResponse GetAllLanguageRegions()
        {
            var response = new GetLanguagesResponse();

            try
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                var languages = cultures
                .OrderBy(x => x.NativeName)
                .Select(x => new LanguageDto
                {
                    IsoCode = x.Name,
                    Name = x.EnglishName,
                    NativeName = x.NativeName
                }).ToList();

                response.Languages = languages;
                response.Success = true;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Common.InternalServerError;
            }

            return response;
        }

        public GetPreferredLanguageResponse GetPreferredLanguage(GetPreferredLanguageRequest request)
        {
            var response = new GetPreferredLanguageResponse();

            if (request.AvailableLanguages.Any())
            {
                var savedValue = this.storageService.GetValue<string>(key);

                if (savedValue.Status == StatusCode.OK && request.AvailableLanguages.Any(x => x.Equals(savedValue.Value, StringComparison.InvariantCultureIgnoreCase)))
                {
                    response.LanguageCode = savedValue.Value;
                }
                else
                {
                    response.LanguageCode = this.GetPreferredLanguage(request.AvailableLanguages, request.DefaultLanguage ?? defaultLanguage);
                }

                response.Status = StatusCode.OK;
            }
            else
            {
                response.Status = StatusCode.BadRequest;
            }

            return response;
        }

        public SetPreferredLanguageResponse SetPreferredLanguage(SetPreferredLanguageRequest request)
        {
            var response = new SetPreferredLanguageResponse();
            var langCode = request.LanguageCode?.Trim().ToLower();

            try
            {
                if (!string.IsNullOrWhiteSpace(langCode) && Regex.IsMatch(langCode, "^[a-z]{2}-[a-z]{2}$"))
                {
                    this.storageService.SaveValue(key, langCode);
                    response.Success = true;
                }
                else
                {
                    response.Status = StatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Common.InternalServerError;
            }

            return response;
        }

        private string GetPreferredLanguage(string[] availableLanguages, string defaultLanguage)
        {
            var browserLanguages = this.context.Request.UserLanguages;

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
