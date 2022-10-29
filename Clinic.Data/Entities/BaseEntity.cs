using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Data.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public AppUser CreatedByItem { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public AppUser UpdatedByItem { get; set; }
        public bool IsDelete { get; set; }

        public BaseEntity(string CreatedBy = null)
        {
            CreatedAt = DateTime.Now;
            this.CreatedBy = CreatedBy;
        }
    }
}
