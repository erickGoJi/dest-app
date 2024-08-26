using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Url
    {
        [DataMember(Name ="data")]
        public Data data { get; set; }
        public class Data
        {
            [DataMember(Name = "full_url")]
            public String full_url { get; set; }

            [DataMember(Name = "url")]
            public String url { get; set; }

            [DataMember(Name = "thumbnails")]
            public List<Thumbnail> thumbnails { get; set; }

            public object embed { get; set; }
        }
    }
}
