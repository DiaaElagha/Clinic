using System.ComponentModel.DataAnnotations;

namespace Clinic.Data.Entities
{
    public class GeneralSettings : BaseEntity
    {
        [Key]
        public int SettingId { get; set; }

        public string GooglePlayAppLink { get; set; }
        public string AppleStoreAppLink { get; set; }
    }
}
