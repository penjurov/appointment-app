using System.Web.Mvc;

namespace Appointment.Web.Areas.Admin.Controllers
{
    public class MenuController : AdminBaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}