using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class BasicCatalogData
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
        [DataMember(Name = "status")]
        public string status { get; set; }
        [DataMember(Name = "created_by")]
        public int? created_by { get; set; }
        [DataMember(Name = "created_on")]
        public DateTime created_on { get; set; }
        [DataMember(Name = "modified_by")]
        public int? modified_by { get; set; }
        [DataMember(Name = "modified_on")]
        public DateTime modified_on { get; set; }
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "value")]
        public string value { get; set; }
    }
}
