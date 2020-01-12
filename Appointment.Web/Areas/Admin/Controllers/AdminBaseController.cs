using Appointment.Common;
using Appointment.Web.Controllers;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public abstract class AdminBaseController : BaseController
    {
    }
}