﻿using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class AppointmentType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Note { get; set; }
        public bool AllowAppointment { get; set; }

        public AppointmentType(string CreatedBy = null) : base(CreatedBy)
        {
            AllowAppointment = true;
        }
    }
}
