namespace MVC.Services.Culture.Transfer
{
    public class GetPreferredLanguageRequest
    {
        public GetPreferredLanguageRequest()
        {
            this.DefaultLanguage = "en-gb";
        }

        public string[] AvailableLanguages { get; set; }

        public string DefaultLanguage { get; set; }
    }
}
