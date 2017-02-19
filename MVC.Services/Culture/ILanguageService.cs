namespace MVC.Services.Culture
{
    using Transfer;

    public interface ILanguageService
    {
        GetLanguageResponse GetLanguage(GetLanguageRequest request);

        GetLanguagesResponse GetAllLanguages();

        GetLanguagesResponse GetAllLanguageRegions();

        GetPreferredLanguageResponse GetPreferredLanguage(GetPreferredLanguageRequest request);

        SetPreferredLanguageResponse SetPreferredLanguage(SetPreferredLanguageRequest request);
    }
}
