using Appointment.Web.App_Start;
using Appointment.Web.Infrastructure.Binders;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Appointment.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEnginesConfiguration.RegisterViewEngines(ViewEngines.Engines);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());
        }
    }
}
