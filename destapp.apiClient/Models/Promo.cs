using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Promo
    {
        [DataMember(Name ="id")]
        public int id { get; set; }
        [DataMember(Name = "status")]
        public String status { get; set; }
        [DataMember(Name = "created_by")]
        public int created_by { get; set; }
        [DataMember(Name = "created_on")]
        public DateTime created_on { get; set; }
        [DataMember(Name = "modified_by")]
        public int modified_by { get; set; }
        [DataMember(Name = "modified_on")]
        public DateTime modified_on { get; set; }
        [DataMember(Name = "name")]
        public String name { get; set; }
        [DataMember(Name = "factor")]
        public int factor { get; set; }
        [DataMember(Name = "start_date")]
        public DateTime start_date { get; set; }
        [DataMember(Name = "end_date")]
        public DateTime end_date { get; set; }
    }
}
