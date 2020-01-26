using System;

namespace Appointment.Data.Proxies
{
    public class AppointmentInfoProxy
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public Guid PartnerId { get; set; }
        public string PartnerName { get; set; }

        public Guid TypeId { get; set; }
        public string TypeName { get; set; }
        public decimal TypeValue { get; set; }

        public DateTime Date { get; set; }

        public decimal Count { get; set; }

        public decimal Total
        {
            get
            {
                return Math.Round(Count * TypeValue, 2);
            }
        }
    }
}
