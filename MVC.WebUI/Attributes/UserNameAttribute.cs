namespace MVC.WebUI.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using Core.Configuration;

    public class UserNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) => value == null || Regex.IsMatch(value.ToString(), RegularExpressions.UserName);
    }
}