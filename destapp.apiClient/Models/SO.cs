using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class SO
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
        [DataMember(Name = "status")]
        public string status { get; set; }
        [DataMember(Name = "created_by")]
        public int? created_by { get; set; }
        [DataMember(Name = "created_on")]
        public DateTime created_on { get; set; }
        [DataMember(Name = "so_id")]
        public int? so_id { get; set; }
        [DataMember(Name = "trivia_id")]
        public int? trivia_id { get; set; }
    }
}
