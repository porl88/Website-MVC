namespace MVC.WebUI.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Core.Configuration;
    using Models.Account;
    using Services.Account;
    using Services.Account.Transfer;
    using Services.Message;
    using Services.Message.Transfer;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IAuthenticationService authenticationService;
        private readonly IMessageService messageService;

        public AccountController(
            IAccountService accountService,
            IAuthenticationService authenticationService,
            IMessageService messageService)
        {
            this.accountService = accountService;
            this.authenticationService = authenticationService;
            this.messageService = messageService;
        }

        // GET: /account
        public ViewResult Index()
        {
            return this.View();
        }

        // GET: /account/create
        [AllowAnonymous]
        public ActionResult Create()
        {
            if (this.authenticationService.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        // POST: /account/create
        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken, ValidateHttpReferrer]
        public ActionResult Create(CreateAccountViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = new CreateAccountRequest
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email,
                    GivenName = model.FirstName,
                    FamilyName = model.LastName
                };

                var response = this.accountService.CreateAccount(request);

                if (response.Success)
                {
                    var messageResponse = this.SendActivateAccountToken(model.Email, response.ActivateAccountToken, model.FirstName, model.LastName);
                    if (messageResponse.Success)
                    {
                        this.TempData["SuccessMessage"] = "You have successfully created a new account. An activation code has been sent to you by email. When you receive the this email, click on the link to activate your account.";
                        return this.RedirectToAction("LogIn");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, response.Message);
                    }
                }
                //else if (response.Status == StatusCode.BadRequest)
                //{
                //    this.ModelState.AddModelError(string.Empty, $"Your account was not created for the following reason: {this.GetErrorMessage(response.CreateAccountStatus)}");
                //}
                else
                {
                    this.ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            return this.View();
        }

        // GET: /account/login
        [AllowAnonymous]
        public ViewResult LogIn(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        // POST: /account/login
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var request = new LoginRequest
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                if (model.Persist)
                {
                    request.Persistence = TimeSpan.FromDays(7);
                }

                var response = this.authenticationService.LogIn(request);

                if (response.Success)
                {
                    return this.SecureRedirect(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            return this.View();
        }

        // GET: /account/logout
        public RedirectToRouteResult LogOut()
        {
            this.authenticationService.LogOut();
            this.TempData["SuccessMessage"] = "You have successfully logged out.";
            return this.RedirectToAction("Index", "Home");
        }

		// GET: /account/activate
		public ActionResult Activate(string activationToken)
		{
			var request = new ActivateAccountRequest();
			var response = this.accountService.ActivateAccount(new ActivateAccountRequest
			{
				ActivateAccountToken = activationToken
			});

			if (response.Success)
			{
				this.TempData["SuccessMessage"] = "Your account has been successfully activated."; // response.Message???
				return this.RedirectToAction("LogIn");
			}
			else
			{
				this.ModelState.AddModelError(string.Empty, response.Message);
			}

			return this.View();
		}

        // GET: /account/change-password
        [ActionName("change-password")]
        public ViewResult ChangePassword()
        {
            return this.View();
        }

        // POST: /account/change-password
        [ActionName("change-password")]
        [HttpPost, ValidateAntiForgeryToken, ValidateHttpReferrer]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = new ChangePasswordRequest
                {
                    UserName = this.authenticationService.CurrentUserName,
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword
                };

                var response = this.accountService.ChangePassword(request);
                if (response.Success)
                {
                    this.TempData["SuccessMessage"] = "You have successfully changed your password.";
                    return this.RedirectToAction("Index");
                }
                else
                {
                    this.ViewBag.DisplaySummary = "yes";
                    this.ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            return this.View();
        }

        // GET: /account/request-password
        [AllowAnonymous]
        [ActionName("request-password")]
        public ViewResult RequestPassword()
        {
            return this.View();
        }

        // POST: /account/request-password
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        [ActionName("request-password")]
        public ActionResult RequestPassword(RequestPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = new RequestPasswordRequest
                {
                    UserName = model.UserName,
                    Expires = TimeSpan.FromHours(2)
                };

                var response = this.authenticationService.ResetPasswordRequest(request);
                if (response.Success)
                {
                    var message = new MessageRequest(response.Email)
                    {
                        Subject = "You have requested to reset your password",
                        Message = response.ResetPasswordToken,
                        FromAddress = Email.DoNotReply
                    };

                    var messageResponse = this.messageService.SendMessage(message);

                    if (messageResponse.Success)
                    {
                        this.TempData["SuccessMessage"] = response.Message;
                        return this.RedirectToAction("Login");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, response.Message);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            return this.View();
        }

        // GET: /account/reset-password
        [AllowAnonymous]
        [ActionName("reset-password")]
        public ActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                this.TempData.Add("ResetPasswordFail", "XXX");
                return this.RedirectToAction("request-password");
            }

            return this.View();
        }

        // POST: /account/reset-password
        [ActionName("reset-password")]
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, ValidateHttpReferrer]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = new ResetPasswordRequest
                {
                    ResetPasswordToken = model.ResetPasswordToken,
                    NewPassword = model.NewPassword
                };

                var response = this.authenticationService.ResetPassword(request);

                if (response.Success)
                {
                    this.TempData["SuccessMessage"] = "You have successfully reset your password. Please login with your new credentials.";
                    return this.RedirectToAction("login");
                }
                else
                {
                    this.TempData.Add("ResetPasswordFail", "XXX");
                    return this.RedirectToAction("request-password");
                }
            }

            return this.View();
        }

        private ActionResult SecureRedirect(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        private MessageResponse SendActivateAccountToken(string email, string activateAccountToken, string firstName, string lastName)
        {
            var fullName = $"{firstName} {lastName}".Trim();

            var emailUrl = this.Url.Action("ActivateAccount", "Email", new
            {
                name = firstName,
                token = activateAccountToken
            });

            var message = new MessageRequest(email, fullName)
            {
                FromAddress = "donotreply@" + Website.CanonicalDomain,
                FromName = Website.Name,
                Subject = "Please activate your account with " + Website.Name,
                MessageUrl = new Uri(emailUrl)
            };

            return this.messageService.SendMessage(message);
        }
    }
}