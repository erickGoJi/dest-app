using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Thumbnail
    {
        [DataMember(Name = "url")]
        public String url { get; set; }

        [DataMember(Name = "relative_url")]
        public String relative_url { get; set; }

        [DataMember(Name = "dimension")]
        public String dimension { get; set; }

        [DataMember(Name = "width")]
        public int width { get; set; }

        [DataMember(Name = "height")]
        public int height { get; set; }
    }
}
