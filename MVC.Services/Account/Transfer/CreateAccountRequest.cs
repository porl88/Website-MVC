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
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool RequireActivation { get; set; }
    }
}
