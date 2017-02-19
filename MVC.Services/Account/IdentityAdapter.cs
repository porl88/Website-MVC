/*
 * TO INSTALL - NuGet packages:
 * Microsoft ASP.NET Identity Core
 * Microsoft ASP.NET Identity Framework (if using Entity Framework)
 * Microsoft ASP.NET Identity Owin
 * Microsoft.Owin.Security.Cookies ???
 */

// https://www.asp.net/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity

// http://tektutorialshub.com/asp-net-identity-tutorial-user-login-and-registration/

// http://www.binaryintellect.net/articles/b957238b-e2dd-4401-bfd7-f0b8d984786d.aspx

namespace MVC.Services.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Data;
    using Core.Exceptions;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Transfer;

    public class IdentityAdapter : IAuthenticationServiceAsync
    {
        private readonly IExceptionHandler exceptionHandler;
        private readonly IUnitOfWork unitOfWork;

        public IdentityAdapter(IUnitOfWork unitOfWork, IExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
            this.unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> LogInAsync(LoginRequest request)
        {
            var response = new LoginResponse();

            try
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = await userManager.FindAsync(request.UserName, request.Password);

                if (user != null)
                {
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    var authenticationProperties = new AuthenticationProperties();

                    if (request.Persistence != null)
                    {
                        authenticationProperties.IsPersistent = true;
                        authenticationProperties.ExpiresUtc = DateTimeOffset.Now.Add((TimeSpan)request.Persistence);
                    }

                    // ASYNC????
                    authenticationManager.SignIn(authenticationProperties, userIdentity);
                    response.Success = true;
                }
                else
                {
                    response.Message = Resources.Account.InvalidUserNameOrPassword;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public void LogOut()
        {
            //var x = new SignInManager<IdentityUser>();
            //x.SignInAsync();
            //x.PasswordSignInAsync();


            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            // ASYNC????
            authenticationManager.SignOut();
        }
    }
}
