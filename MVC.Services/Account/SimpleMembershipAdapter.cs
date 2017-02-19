namespace MVC.Services.Account
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Security; // in System.Web.ApplicationServices
    using Core.Data;
    using Core.Exceptions;
    using Transfer;
    using WebMatrix.WebData;

    /***********************************************************************************************************************

    nuget: install-package Microsoft.AspNet.WebPages.WebData

    Membership: uses System.Web.Security which is in System.Web.ApplicationServices

    in Global.asax:

    if (!WebSecurity.Initialized)
    {
        WebSecurity.InitializeDatabaseConnection("YOUR_DB_CONTEXT", "USER_TABLE", "ID_COLUMN", "USERNAME_COLUMN", true);
    }

    *************************************************************************************************************************/

    public class SimpleMembershipAdapter : IAuthenticationService, IAccountService, IUserService
    {
        private readonly HttpContextBase context;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IUnitOfWork unitOfWork;

        public SimpleMembershipAdapter(HttpContextBase context, IUnitOfWork unitOfWork, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.exceptionHandler = exceptionHandler;
            this.unitOfWork = unitOfWork;
        }

        public bool IsAuthenticated => WebSecurity.IsAuthenticated;

        public int CurrentUserId => WebSecurity.CurrentUserId;

        public string CurrentUserName => WebSecurity.CurrentUserName;

        public LoginResponse LogIn(LoginRequest request)
        {
            var response = new LoginResponse();

            try
            {
                var persist = request.Persistence != null;

                if (WebSecurity.Login(request.UserName, request.Password, persist))
                {
                    if (persist)
                    {
                        // this does not work, as the expiration date gets overridden by the timeout value of the <form> section in the web.config file and there is no way of programmatically changing/overriding it
                        this.context.Response.Cookies[0].Expires = DateTime.Now.Add((TimeSpan)request.Persistence);
                    }

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

        public void LogOut() => WebSecurity.Logout();

        public RequestPasswordResponse ResetPasswordRequest(RequestPasswordRequest request)
        {
            var response = new RequestPasswordResponse();

            try
            {
                if (WebSecurity.UserExists(request.UserName))
                {
                    response.ResetPasswordToken = WebSecurity.GeneratePasswordResetToken(request.UserName, (int)request.Expires.TotalMinutes);
                    response.Success = true;
                }
                else
                {
                    response.Message = Resources.Account.InvalidUserName;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();

            try
            {
                if (WebSecurity.ResetPassword(request.ResetPasswordToken, request.NewPassword))
                {
                    response.Success = true;
                }
                else
                {
                    response.Message = Resources.Account.InvalidToken;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public ActivateAccountResponse ActivateAccount(ActivateAccountRequest request)
        {
            var response = new ActivateAccountResponse();

            try
            {
                if (WebSecurity.ConfirmAccount(request.ActivateAccountToken))
                {
                    response.Success = true;
                }
                else
                {
                    response.Message = Resources.Account.InvalidToken;
                }
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            var response = new ChangePasswordResponse();

            try
            {
                if (WebSecurity.ChangePassword(request.UserName, request.OldPassword, request.NewPassword))
                {
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

        public CreateAccountResponse CreateAccount(CreateAccountRequest request)
        {
            var response = new CreateAccountResponse();

            try
            {
                var propertyValues = new
                {
                    Email = request.Email
                };

                response.ActivateAccountToken = WebSecurity.CreateUserAndAccount(request.UserName, request.Password, propertyValues, requireConfirmationToken: request.RequireActivation);
                response.Success = true;
            }
            // https://msdn.microsoft.com/en-us/library/system.web.security.membershipcreateuserexception.statuscode(v=vs.110).aspx
            catch (MembershipCreateUserException ex)
            {
                response.Message = this.GetErrorMessage(ex.StatusCode);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public DeleteAccountResponse DeleteAccount(DeleteAccountRequest request)
        {
            var response = new DeleteAccountResponse();

            try
            {
                var userName = request.UserName;
                var roles = Roles.GetRolesForUser(userName);
                if (roles.Any())
                {
                    Roles.RemoveUserFromRoles(userName, roles);
                }

                var membership = (SimpleMembershipProvider)Membership.Provider;
                membership.DeleteAccount(userName); // deletes record from webpages_Membership table
                membership.DeleteUser(userName, true); // deletes record from UserProfile table

                response.Success = true;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            var response = new CreateUserResponse();

            try
            {
                var user = request.User;
                var password = request.Password ?? Membership.GeneratePassword(14, 5);
                var now = DateTimeOffset.Now;

                var propertyValues = new
                {
                    FirstName = user.FirstName.Trim(),
                    LastName = user.LastName.Trim(),
                    Email = user.Email.Trim().ToLower(),
                    Created = now,
                    Updated = now
                };

                WebSecurity.CreateUserAndAccount(user.UserName.Trim(), password, propertyValues, requireConfirmationToken: false);
                response.Password = password;
                response.Success = true;
            }
            // https://msdn.microsoft.com/en-us/library/system.web.security.membershipcreateuserexception.statuscode(v=vs.110).aspx
            catch (MembershipCreateUserException ex)
            {
                response.Message = this.GetErrorMessage(ex.StatusCode);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        public GetUsersResponse GetUsers(GetUsersRequest request)
        {
            var response = new GetUsersResponse();

            try
            {
                response.Users = this.unitOfWork.UserRepository.Get(q => q.Select(x => new UserDto
                {
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                }));

                response.Success = true;
            }
            catch (Exception ex)
            {
                this.exceptionHandler.HandleException(ex);
                response.Message = Resources.Account.InternalServerError;
            }

            return response;
        }

        private string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.Account.DuplicateUserName;
                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.Account.DuplicateEmail;
                case MembershipCreateStatus.InvalidUserName:
                    return Resources.Account.InvalidUserName;
                case MembershipCreateStatus.InvalidPassword:
                    return Resources.Account.InvalidPassword;
                case MembershipCreateStatus.InvalidEmail:
                    return Resources.Account.InvalidEmail;
                case MembershipCreateStatus.InvalidAnswer:
                    return Resources.Account.InvalidAnswer;
                case MembershipCreateStatus.InvalidQuestion:
                    return Resources.Account.InvalidQuestion;
                case MembershipCreateStatus.ProviderError:
                    return Resources.Account.ProviderError;
                case MembershipCreateStatus.UserRejected:
                    return Resources.Account.UserRejected;
                default:
                    return Resources.Account.UnknownError;
            }
        }
    }
}
