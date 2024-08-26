using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class BadgesResponse
    {
        public List<BadgesDatum> data { get; set; }

        public class BadgesDatum
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public AvatarResponse.CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public AvatarResponse.ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public string title { get; set; }
            public Url image { get; set; }
            public string description { get; set; }
            public bool isSelected { get; set; }
            public bool isGetting { get; set; }
        }
    }
}
