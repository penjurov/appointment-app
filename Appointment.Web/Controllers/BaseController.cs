using Appointment.Web.Infrastructure.ActionResult;
using System.Web.Mvc;

namespace Appointment.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return new DateTimeJsonResult(data, behavior);
        }
    }
}