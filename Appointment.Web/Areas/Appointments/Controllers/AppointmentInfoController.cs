using Appointment.Data.Filters;
using Appointment.Data.Proxies;
using Appointment.Web.Areas.Appointments.Services;
using Appointment.Web.Models;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Appointment.Controllers
{
    public class AppointmentInfoController : AppointmentBaseController
    {
        [Dependency]
        public AppointmentInfoService Service { get; set; }

        public ActionResult Index()
        {
            var model = Service.BuildModel();
            return this.View((object)JsonConvert.SerializeObject(model));
        }

        public ActionResult List()
        {
            var model = Service.BuildModel();
            return this.View((object)JsonConvert.SerializeObject(model));
        }

        [HttpPost]
        public JsonResult Get(AppointmentInfoFilter filter)
        {
            var response = new ResponseModel();

            try
            {
                response.Data = new
                {
                    Records = Service.Get(filter),
                    Count = filter.Count,
                    TotalValue = filter.TotalValue
                };
            }
            catch (Exception ex)
            {
                response.SetException(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(AppointmentInfoProxy proxy)
        {
            var response = new ResponseModel();

            try
            {
                Service.Save(proxy);
            }
            catch (Exception ex)
            {
                response.SetException(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}