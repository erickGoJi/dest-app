using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models.Torneo
{
    [DataContract]
    public class TorneoResponse
    {
        [DataMember(Name = "data")]
        public List<Torneo> data { get; set; }

    }
}
