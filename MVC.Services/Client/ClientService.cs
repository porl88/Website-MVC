namespace MVC.Services.Client
{
    using System.Linq;
    using System.Web;
    using Core.Exceptions;

    public class ClientService : IClientService
    {
        private readonly HttpContextBase context;
        private readonly IExceptionHandler exceptionHandler;

        public ClientService(HttpContextBase context, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.exceptionHandler = exceptionHandler;
        }

        public string GetPreferredLanguage()
        {
            return this.context.Request.UserLanguages.FirstOrDefault();
        }
    }
}
