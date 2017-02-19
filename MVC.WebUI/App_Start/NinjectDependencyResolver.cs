namespace MVC.WebUI
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Core.Configuration;
    using Core.Data;
    using Core.Data.EntityFramework;
    using Core.Exceptions;
    using Ninject;
    using Services.Account;
    using Services.Article;
    using Services.Culture;
    using Services.Message;
    using Services.Page;
    using Services.Storage;

    public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;

		public NinjectDependencyResolver()
		{
			this.kernel = new StandardKernel();
			this.AddBindings();
		}

		public object GetService(Type serviceType)
		{
			return this.kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return this.kernel.GetAll(serviceType);
		}

		private void AddBindings()
		{
            this.kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            this.kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            this.kernel.Bind<HttpServerUtilityBase>().ToMethod(ctx => new HttpServerUtilityWrapper(HttpContext.Current.Server)).InTransientScope();
            this.kernel.Bind<IAccountService>().To<SimpleMembershipAdapter>();
            this.kernel.Bind<IAuthenticationService>().To<SimpleMembershipAdapter>();
            this.kernel.Bind<IArticleService>().To<ArticleService>();
#if DEBUG
            this.kernel.Bind<IExceptionHandler>().To<NullExceptionHandler>();
#else
			this.kernel.Bind<IExceptionHandler>().To<EmailExceptionHandler>();
#endif
            this.kernel.Bind<ILanguageService>().To<LanguageService>();
            this.kernel.Bind<IMessageService>().To<EmailService>();
            this.kernel.Bind<IPageService>().To<PageService>();
            this.kernel.Bind<ILocalStorageService>().To<CookieStorageService>();
            this.kernel.Bind<ISystemSettings>().To<SystemSettings>();
            this.kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //this.kernel.BindFilter<GlobalErrorAttribute>(FilterScope.Action, 0);
        }
    }
}