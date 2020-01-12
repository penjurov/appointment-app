﻿using System;

namespace Appointment.Models.Contracts
{
    public class DeletableEntity : IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
