namespace MVC.WebUI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Attributes;

    public class ChangePasswordViewModel
	{
        [Display(Name = "PasswordOld", ResourceType = typeof(Resources.FormLabels))]
        public string OldPassword { get; set; }

        [Display(Name = "PasswordNew", ResourceType = typeof(Resources.FormLabels))]
        [Required, Password(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ValidationErrorMessages))]
        public string NewPassword { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.FormLabels))]
        [Compare("Password", ErrorMessageResourceName = "PasswordConfirm", ErrorMessageResourceType = typeof(Resources.ValidationErrorMessages))]
        public string ConfirmPassword { get; set; }
	}
}