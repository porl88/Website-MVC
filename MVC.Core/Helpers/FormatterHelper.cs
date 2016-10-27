namespace MVC.Core.Helpers
{
    using System;

    public static class FormatterHelper
    {
        public static string Email(string text) => text?.ToLowerInvariant().Trim();

        public static string UkPostCode(string text)
        {
            var output = text.Replace(" ", string.Empty);

            if (output.Length > 3)
            {
                return output.Insert(output.Length - 3, " ").ToUpperInvariant();
            }

            return text;
        }

        public static string Url(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url.Trim(), UriKind.Absolute, out uri))
            {
                return uri.AbsoluteUri;
            }

            return url;
        }
    }
}
