namespace MVC.Services.Account.Transfer
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAccountRequest
    {
        public CreateAccountRequest()
        {
            this.RequireActivation = true;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

		[Required]
		public string FullName { get; set; }

		public bool RequireActivation { get; set; }
    }
}
