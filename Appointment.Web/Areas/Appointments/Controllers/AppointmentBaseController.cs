using Appointment.Web.Controllers;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Appointment.Controllers
{
    [Authorize]
    public abstract class AppointmentBaseController : BaseController
    {
    }
}