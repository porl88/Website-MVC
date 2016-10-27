namespace MVC.Services.Culture.Transfer
{
    public class LanguageDto
    {
        public int Id { get; set; }

        public string LanguageCode { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsSelected { get; set; }
    }
}
