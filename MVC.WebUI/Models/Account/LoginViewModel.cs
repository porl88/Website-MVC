namespace MVC.WebUI.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
	{
        [Display(Name = "UserName", ResourceType = typeof(Resources.FormLabels))]
        [Required]
		public string UserName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.FormLabels))]
        [Required]
        public string Password { get; set; }

        [Display(Name = "LoginPersist", ResourceType = typeof(Resources.FormLabels))]
        public bool Persist { get; set; }
	}
}