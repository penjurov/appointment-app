using Appointment.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Appointment.Models
{
    public class AppointmentType: DeletableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
