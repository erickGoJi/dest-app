using System;
using System.Collections.Generic;
using System.Text;
using static destapp.apiClient.Models.AvatarResponse;

namespace destapp.apiClient.Models
{
    public class TypeNotificationResponse
    {
        public List<TypeNotificationDatum> data { get; set; }

        public class TypeNotificationDatum
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public Url image { get; set; }
            public string notification_type { get; set; }
            public Url icon { get; set; }
            public string deeplink_app { get; set; }
            public string deeplink_web { get; set; }
        }
    }
}
