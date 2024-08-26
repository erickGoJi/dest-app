using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class State
    {
        [DataMember(Name = "state_id")]
        public BasicCatalogData state_id { get; set; }
    }
}
