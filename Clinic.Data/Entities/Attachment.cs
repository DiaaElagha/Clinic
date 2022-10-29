using System;
using System.ComponentModel.DataAnnotations;
namespace Clinic.Data.Entities
{
    public class Attachment : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string BaseName { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public Attachment(string CreatedBy = null) : base(CreatedBy)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
