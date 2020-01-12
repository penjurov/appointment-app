using Appointment.Common;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return GlobalConstants.AdminAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Menu", action = "Index", id = UrlParameter.Optional });
        }
    }
}