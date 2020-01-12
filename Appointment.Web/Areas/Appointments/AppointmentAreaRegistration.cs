using Appointment.Common;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Appointment
{
    public class AppointmentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return GlobalConstants.AppointmentAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Appointments_default",
                "Appointments/{controller}/{action}/{id}",
                new { controller = "Menu", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}