namespace MVC.WebUI.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class NameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }
}