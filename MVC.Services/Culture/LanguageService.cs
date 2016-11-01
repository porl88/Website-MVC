namespace MVC.Services.Culture
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Data;
    using Core.Entities.Culture;
    using Core.Exceptions;
    using Storage;
    using Transfer;

    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Language> languageRepository;
        private readonly IExceptionHandler exceptionHandler;
        private readonly ILocalStorageService storageService;
        private readonly HttpContextBase context;
        private const string defaultLanguage = "en-gb";
        private const string key = "lang";

        public LanguageService(IUnitOfWork unitOfWork, IExceptionHandler exceptionHandler, ILocalStorageService storageService, HttpContextBase context)
        {
            this.unitOfWork = unitOfWork;
            this.languageRepository = unitOfWork.LanguageRepository;
            this.exceptionHandler = exceptionHandler;
            this.storageService = storageService;
            this.context = context;
        }

        public GetLanguagesResponse GetLanguages(GetLanguagesRequest request = null)
        {
            var response = new GetLanguagesResponse();

            try
            {
                response.Languages = this.unitOfWork.LanguageRepository.Get(q => q
                    .Where(x => x.Active)
                    .OrderBy(x => x.LocalName)
                    .Select(x => new LanguageDto
                    {
                        Id = x.Id,
                        LanguageCode = x.LanguageCode,
                        Name = x.LocalName
                    })
                );

                response.Status = StatusCode.OK;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Status = StatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<GetLanguagesResponse> GetLanguagesAsync(GetLanguagesRequest request)
        {
            var response = new GetLanguagesResponse();

            try
            {
                response.Languages = await this.unitOfWork.LanguageRepository.GetAsync(q => q
                    .Where(x => x.Active)
                    .OrderBy(x => x.LocalName)
                    .Select(x => new LanguageDto
                    {
                        Id = x.Id,
                        LanguageCode = x.LanguageCode,
                        Name = x.LocalName
                    })
                );

                response.Status = StatusCode.OK;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Status = StatusCode.InternalServerError;
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
                    response.Status = StatusCode.OK;
                }
                else
                {
                    response.Status = StatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.Status = StatusCode.InternalServerError;
                this.exceptionHandler.HandleException(ex);
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
