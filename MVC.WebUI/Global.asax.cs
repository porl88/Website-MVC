﻿namespace MVC.WebUI
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using WebMatrix.WebData;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            this.SetViewEngines();
            this.Optimise();

            //GlobalConfiguration.Configure(WebApiConfig.Register); // enables Web Api - position is important - needs to come before RegisterRoutes
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
#if DEBUG
            Database.SetInitializer(new DatabaseInitializer());; // Recreates database with test data. A call needs to be made to the database before this will run. N.B. This can alternatively be configured in the configuration/entityFramework/contexts section of the web.config file
            // does not work if Database.SetInitializer is set in Global.asax.cs as well
#endif
        }

        private void SetViewEngines()
        {
            // Remove unused view engines - http://www.codeproject.com/Articles/635324/Another-set-of-ASP-NET-MVC-4-tips#tip-17
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        private void Optimise()
        {
            // Disable MVC response header - http://www.codeproject.com/Articles/635324/Another-set-of-ASP-NET-MVC-4-tips#tip-17
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}
