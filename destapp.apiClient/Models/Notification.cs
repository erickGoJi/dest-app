using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class NotificationDatum
    {
        [DataMember(Name = "notification_id")]
        public NotificationData notification_id { get; set; }
        [DataContract]
        public class NotificationData : BasicCatalogData
        {
            [DataMember(Name ="title")]
            public string title { get; set; }
            [DataMember(Name = "description")]
            public string decription { get; set; }
            [DataMember(Name = "image")]
            public int? image { get; set; }
            [DataMember(Name = "notification_type")]
            public string notification_type { get; set; }
            [DataMember(Name = "icon")]
            public int? icon { get; set; }
            [DataMember(Name = "deeplink_app")]
            public string deeplink_app { get; set; }
            [DataMember(Name = "deeplink_web")]
            public string deeplink_web { get; set; }
        }
    }
}
