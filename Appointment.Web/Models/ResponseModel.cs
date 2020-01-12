using Appointment.Web.Infrastructure.Exceptions;
using System;

namespace Appointment.Web.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel()
        {
            Success = true;
        }

        public void SetException(Exception ex)
        {
            Success = false;
            if (ex is UnauthorizedAccessException || ex is UserException)
            {
                Message = ex.Message;
            }
            else
            {
                Message = "An unexpected error has occurred";
            }
        }
    }
}