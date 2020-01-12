using System;

namespace Appointment.Web.Infrastructure.Exceptions
{
    public class UserException: Exception
    {
        public UserException(string message) : base(message)
        {
        }
    }
}