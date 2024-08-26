using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class InteresesResponse
    {
        public List<InteresesDatum> data { get; set; }
        public class InteresesDatum
        {
            public int id { get; set; }
            public string status { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public string name { get; set; }
            public string value { get; set; }
            public object image { get; set; }
            public bool isSelected { get; set; }
        }
    }
}
