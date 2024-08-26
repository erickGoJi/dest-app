using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Gender
    {
        [DataMember(Name = "gender_id")]
        public BasicCatalogData gender_id { get; set; }

    }
}
