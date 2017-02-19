namespace MVC.Services.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Transfer;

    public interface IAuthenticationServiceAsync
    {
        Task<LoginResponse> LogInAsync(LoginRequest request);

        void LogOut();
    }
}
