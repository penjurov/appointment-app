using Appointment.Data.Filters;
using Appointment.Data.Proxies;
using Appointment.Web.Areas.Admin.Services;
using Appointment.Web.Models;
using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;

namespace Appointment.Web.Areas.Admin.Controllers
{
    public class PartnersController : AdminBaseController
    {
        [Dependency]
        public PartnerService Service { get; set; }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public JsonResult Get(AdminFilter filter)
        {
            var response = new ResponseModel();

            try
            {
                response.Data = Service.Get(filter);
            }
            catch (Exception ex)
            {
                response.SetException(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

            //return this.Json(new { records = result, total = filter.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Upsert(PartnerProxy proxy)
        {
            var response = new ResponseModel();

            try
            {
                response.Data = Service.Upsert(proxy);
            }
            catch (Exception ex)
            {
                response.SetException(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove(Guid id)
        {
            var response = new ResponseModel();

            try
            {
                Service.Remove(id);
            }
            catch (Exception ex)
            {
                response.SetException(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}