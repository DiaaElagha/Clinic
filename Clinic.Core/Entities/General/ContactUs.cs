using Clinic.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.Entities
{
    public class ContactUs : BaseEntity
    {
        public ContactUs(string CreatedBy = null) : base(CreatedBy)
        {
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Messege { get; set; }
    }
}
