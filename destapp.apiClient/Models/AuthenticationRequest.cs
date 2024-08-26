using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    class AuthenticationRequest
    {
        [DataMember(Name ="email")]
        public string email { get; set; }
        [DataMember(Name ="password")]
        public string password { get; set; }
    }
}
