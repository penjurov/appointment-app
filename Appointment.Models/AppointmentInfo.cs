using Appointment.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Appointment.Models
{
    public class AppointmentInfo: DeletableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Count { get; set; }

        [Required]
        public Guid TypeId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid PartnerId { get; set; }

        public virtual AppointmentType Type { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Partner Partner { get; set; }
    }
}
