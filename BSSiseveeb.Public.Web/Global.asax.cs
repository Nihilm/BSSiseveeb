﻿using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BSSiseveeb.Core;
using BSSiseveeb.Public.Web.Windsor;
using Castle.Windsor;
using Castle.Windsor.Installer;
using BSSiseveeb.Public.Web.ScheduledTasks;

namespace BSSiseveeb.Public.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new WindsorContainer()
                .Install(FromAssembly.This());

            IoC.Setup(x => container.Resolve(x));
            IoC.SetContainer(container);

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Scheduler.Start();
        }
    }
}
