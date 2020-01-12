using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Appointment.Web.Infrastructure.ActionResult
{
    public class DateTimeJsonResult : JsonResult
    {
        public DateTimeJsonResult(object data)
        {
            this.Data = data;
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public DateTimeJsonResult(object data, JsonRequestBehavior behavior)
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                response.Write(this.Serialize(this.Data));
            }
        }

        public string Serialize(object value)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                Converters = new JsonConverter[] { new IsoDateTimeConverter { Culture = CultureInfo.InvariantCulture, DateTimeFormat = "dd/MM/yyyy H:mm tt" } }
            };
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}