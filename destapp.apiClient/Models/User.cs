using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
    }
}
