using System.Collections.Generic;

namespace Appointment.Data.Proxies
{
    public class AppointmentModel
    {
        public IEnumerable<EmployeeProxy> Employees { get; set; }
        public IEnumerable<PartnerProxy> Partners { get; set; }
        public IEnumerable<AppointmentTypeProxy> Types { get; set; }
    }
}
