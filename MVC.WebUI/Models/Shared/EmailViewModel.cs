namespace MVC.WebUI.Models.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Attributes;
    using Core.Helpers;
    using Resources;

    public class EmailViewModel
	{
		private string firstName;

		private string lastName;

		private string email;

		private string subject;

        public string EmailPattern { get; set; }

		[Display(Name = "Your First Name")]
        [Required, MaxLength(30), Name(ErrorMessageResourceName = "Name", ErrorMessageResourceType = typeof(ValidationErrorMessages))]
        public string FirstName
		{
			get
			{
				return this.firstName;
			}

			set
			{
				this.firstName = value.Trim();
			}
		}

		[Display(Name = "Your Last Name")]
        [Required, MaxLength(30), Name(ErrorMessageResourceName = "Name", ErrorMessageResourceType = typeof(ValidationErrorMessages))]
        public string LastName
		{
			get
			{
				return this.lastName;
			}

			set
			{
				this.lastName = value.Trim();
			}
		}

		[Display(Name = "Your Email")]
        [Required, MaxLength(256), EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(ValidationErrorMessages))]
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

		[Required, MaxLength(100)]
		public string Subject
		{
			get
			{
				return this.subject;
			}

			set
			{
				this.subject = value.Trim();
			}
		}

		[Required, MaxLength(2000)]
		public string Message { get; set; }
	}
}