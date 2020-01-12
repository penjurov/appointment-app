using System;

namespace Appointment.Data.Proxies
{
    public class AppointmentTypeProxy
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
