namespace MVC.WebUI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Attributes;
    using Core.Helpers;

    public class CreateAccountViewModel
	{
		private string email;

		[Display(Name = "UserName", ResourceType = typeof(Resources.FormLabels))]
		[Required, MaxLength(30)]
		public string UserName { get; set; }

        [Required, MaxLength(256), EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(Resources.ValidationErrorMessages))]
        public string Email
		{
			get
			{
				return this.email;
			}

			set
			{
				this.email = FormatterHelper.Email(value);
			}
		}

        [Display(Name = "Password", ResourceType = typeof(Resources.FormLabels))]
        [Required, Password(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ValidationErrorMessages))]
        public string Password { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.FormLabels))]
        [Compare("Password", ErrorMessageResourceName = "PasswordConfirm", ErrorMessageResourceType = typeof(Resources.ValidationErrorMessages))]
		public string ConfirmPassword { get; set; }
	}
}