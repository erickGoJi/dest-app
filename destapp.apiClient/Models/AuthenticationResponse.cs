using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class AuthenticationResponse
    {
        [DataMember(Name = "data")]
        public Data data { get; set; }
        [DataMember(Name = "public")]
        public bool publico{ get; set;}
        public class Data
        {
            public String token { get; set; }
        }

    }
}
