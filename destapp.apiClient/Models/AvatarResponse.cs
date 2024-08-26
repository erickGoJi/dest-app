using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class AvatarResponse
    {
        public List<Datum> data { get; set; }

        public class CreatedBy
        {
            public int id { get; set; }
        }

        public class ModifiedBy
        {
            public int id { get; set; }
        }

        public class Datum
        {
            public int id { get; set; }
            public string status { get; set; }
            public CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public Url image { get; set; }
            public bool isSelected { get; set; }

            public Datum()
            {
                created_by = new CreatedBy();
                modified_by = new ModifiedBy();
                image = new Url();
            }
        }
    }
}
