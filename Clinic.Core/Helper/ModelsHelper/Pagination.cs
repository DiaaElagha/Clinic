using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Clinic.Core.Helper.ModelsHelper
{
    public class Pagination
    {
        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int CurrentPage { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }
        public int TotalPages { get; set; } // this is the only property that is assigned
    }
}
