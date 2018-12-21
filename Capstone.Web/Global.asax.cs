using Ninject;
using Ninject.Web.Common.WebHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WorkoutService;

namespace Capstone.Web
{
    public class MvcApplication : NinjectHttpApplication
    { 
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // Bind Database
            string connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
            //kernel.Bind<IWorkoutDAL>().To<MockDAL>();

            kernel.Bind<IWorkoutDAL>().To<WorkoutDAL>().WithConstructorArgument("connectionString", connectionString);

            return kernel;
        }
    }
}
