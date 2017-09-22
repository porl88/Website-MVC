namespace MVC.WebUI.Models.Account
{
	using System.ComponentModel.DataAnnotations;

	public class ActivateAccountViewModel
	{
		[Required, EmailAddress, MaxLength(256)] 
		public string Email { get; set; }
	}
}