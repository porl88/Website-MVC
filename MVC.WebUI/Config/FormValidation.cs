namespace MVC.WebUI.Config
{
    using Core.Configuration;

    internal static class FormValidation
    {
        public static string UkPostCode => @" *[a-zA-Z]{1,2}\d[a-zA-Z\d]? *\d[a-zA-Z]{2} *";

        public static string Name => @"(?=.*\w{2})[a-zA-Z '-]+";

        public static string DomainName => @" *(https?://\.)?(www\.)?(?!www\.)[a-z0-9-]+(\.[a-z]+)+ *";

        public static string Password => RegularExpressions.PasswordWeak;
    }
}