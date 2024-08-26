using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class PushNotificationResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<Results> results { get; set; }

        public class Results
        {
            public string message_id { get; set; }
        }

        public PushNotificationResponse()
        {
            results = new List<Results>();
        }
    }
}
