namespace MVC.WebUI.Models.Shared
{
    using System.Collections.Generic;
    using Services.Culture.Transfer;

    public class LanguageListViewModel
    {
        public LanguageListViewModel()
        {
            this.Languages = new List<LanguageDto>();
        }

        public string LanguageCode { get; set; }

        public List<LanguageDto> Languages { get; set; }
    }
}