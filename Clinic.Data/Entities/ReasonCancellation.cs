using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class ReasonCancellation : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public ReasonCancellation(string CreatedBy = null) : base(CreatedBy)
        {
        }
    }
}
