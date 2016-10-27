namespace MVC.Services.Account.Transfer
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAccountRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
