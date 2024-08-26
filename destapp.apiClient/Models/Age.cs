using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Age
    {
        [DataMember(Name = "age_range_id")]
        public BasicCatalogData age_range_id { get; set; }
    }
}
