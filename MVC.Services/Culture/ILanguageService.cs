namespace MVC.Services.Culture
{
    using System.Threading.Tasks;
    using Transfer;

    public interface ILanguageService
    {
        GetLanguagesResponse GetLanguages(GetLanguagesRequest request = null);

        Task<GetLanguagesResponse> GetLanguagesAsync(GetLanguagesRequest request);

        GetPreferredLanguageResponse GetPreferredLanguage(GetPreferredLanguageRequest request);

        SetPreferredLanguageResponse SetPreferredLanguage(SetPreferredLanguageRequest request);
    }
}
