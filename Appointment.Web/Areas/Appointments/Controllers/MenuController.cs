using System.Web.Mvc;

namespace Appointment.Web.Areas.Appointment.Controllers
{
    public class MenuController: AppointmentBaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}