using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
