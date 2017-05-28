namespace MVC.WebUI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Attributes;

    public class ResetPasswordViewModel
	{
        [Display(Name = "PasswordNew", ResourceType = typeof(Resources.FormLabels))]
        [Required, Password(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
		public string NewPassword { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.FormLabels))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordConfirm", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
		public string ConfirmPassword { get; set; }

        public string ResetPasswordToken { get; set; }
	}
}