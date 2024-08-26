using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Interest
    {
        [DataMember(Name ="interests_id")]
        public BasicCatalogData interests_id { get; set; }
    }
}
