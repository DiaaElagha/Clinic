using Clinic.Core.Constant;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public UserRoles? Role { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string LandPhone { get; set; }
        public string Address { get; set; }

        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public AppUser CreatedByItem { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public AppUser UpdatedByItem { get; set; }

        public AppUser(string CreatedBy = null)
        {
            this.CreatedAt = DateTime.Now;
            this.CreatedBy = CreatedBy;
            this.IsActive = false;
        }
    }
}
