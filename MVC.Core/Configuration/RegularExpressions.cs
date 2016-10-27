namespace MVC.Core.Configuration
{
    public static class RegularExpressions
    {
        //public const string Email = @"^(?!.{256})[a-zA-Z\d!#\$%&'\*\+\-/=\?\^_`\{\|}~]+(\.[a-zA-Z\d!#\$%&'\*\+\-/=\?\^_`\{\|}~]+)*@[a-zA-Z\d]+([\.\-][a-zA-Z\d]+)*(\.[a-zA-Z]{2,})$";

        // http://w3c.github.io/html-reference/input.email.html
        public static string Email => @"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

        public static string UkPostCode => @"^[A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2}$";

        public static string Name => @"^(?=.*\w{2})[a-zA-Z '-]+$"; // must contain at least 2 consecutive letters

        public static string DomainName => @"^(https?://\.)?(www\.)?(?!www\.)[a-z0-9-]+(\.[a-z]+)+$";

        public static string Uri => $@"{DomainName.TrimEnd('$')}(/[\w-])*(\?[^ ]*)+$";

        public static string PasswordWeak => @"^\S{8,}$"; // min 8 characters, no white space characters

        public static string PasswordMedium => @"^(?=(.*[a-z]){2})(?=(.*[A-Z\d\W_]){2})\S{8,}$"; // min 8 characters, no white space, at least 2 lower case characters, must contain at least 2 of ANY combination of the following: uppercase characters OR digits OR special characters (e.g. ~$%#)

        public static string PasswordStrong => @"^(?=.*[a-z])(?=.*[A-Z])(?=(.*\d))\S{8,}$"; // min 8 characters, no white space, must contain at least 1 of EACH the following characters: lowercase characters AND uppercase characters AND digits

        public static string PasswordBest => @"^(?=.*[a-z])(?=.*[A-Z])(?=(.*\d))(?=.*[\W_])\S{14,}$"; // min 14 characters, no white space, must contain at least 1 of EACH the following characters: lowercase characters AND uppercase characters AND digits AND special characters (e.g. ~$%#)
    }
}
