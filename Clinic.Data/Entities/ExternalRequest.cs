using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class ExternalRequest : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string RequestUrl { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public string RequestMethod { get; set; }
        public string ResponseBody { get; set; }

        public ExternalRequest(string CreatedBy = null) : base(CreatedBy)
        {
        }
    }
}
