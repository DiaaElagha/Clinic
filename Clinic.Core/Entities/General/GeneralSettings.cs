using Clinic.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class GeneralSettings : BaseEntity
    {
        [Key]
        public int SettingId { get; set; }

        public string GooglePlayAppLink { get; set; }
        public string AppleStoreAppLink { get; set; }
    }
}
