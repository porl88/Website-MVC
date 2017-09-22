namespace MVC.WebUI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Attributes;

    public class CreateAccountViewModel
	{
		[Display(Name = "UserName", ResourceType = typeof(Resources.FormLabels))]
        [Required, MaxLength(30), UserName]
		public string UserName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.FormLabels))]
        [Required, MaxLength(256), EmailAddress] // N.B. EmailAddress has built-in language support
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.FormLabels))]
        [Required, MaxLength(200), Password(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Password { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.FormLabels))]
        [Compare("Password")] // N.B. Compare has built-in language support
		public string PasswordConfirm { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.FormLabels))]
        [Required, MaxLength(30), Name]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources.FormLabels))]
        [Required, MaxLength(50), Name]
        public string LastName { get; set; }

		[Display(Name = "FullName", ResourceType = typeof(Resources.FormLabels))]
		[Required, MaxLength(80), Name]
		public string FullName { get; set; }
	}
}