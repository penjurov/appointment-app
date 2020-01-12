using System;

namespace Appointment.Data.Filters
{
    public class AppointmentInfoFilter: PagingFilter
    {
        public Guid? EmployeeId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? TypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal TotalValue { get; set; }
    }
}
