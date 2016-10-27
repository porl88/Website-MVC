namespace MVC.WebUI.Models.Account
{
	using System.ComponentModel.DataAnnotations;

	public class RequestPasswordViewModel
	{
        [Display(Name = "UserName", ResourceType = typeof(Resources.FormLabels))]
        [Required]
		public string UserName { get; set; }
	}
}