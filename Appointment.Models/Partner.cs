using Appointment.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Appointment.Models
{
    public class Partner: DeletableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
