using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static destapp.apiClient.Models.HomeResponse;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class AccessCode
    {
        [DataMember(Name ="code_id")]
        public CodeDatum code_id { get; set; }
        [DataContract]
        public class CodeDatum
        {
            [DataMember(Name = "id")]
            public int id { get; set; }

            [DataMember(Name = "status")]
            public string status { get; set; }

            [DataMember(Name = "created_by")]
            public CreatedBy2 created_by { get; set; }

            [DataMember(Name = "created_on")]
            public DateTime created_on { get; set; }

            [DataMember(Name = "modified_by")]
            public CreatedBy2 modified_by { get; set; }

            [DataMember(Name = "modified_on")]
            public DateTime modified_on { get; set; }

            [DataMember(Name = "name")]
            public string name { get; set; }

            [DataMember(Name = "type")]
            public List<string> type { get; set; }

            [DataMember(Name = "coins_to_add")]
            public int? coins_to_add { get; set; }

            [DataMember(Name = "tickets_to_add")]
            public int? tickets_to_add { get; set; }

            [DataMember(Name = "image")]
            public Url image { get; set; }

            [DataMember(Name ="brand")]
            public Brand brand { get; set; }

            [DataMember(Name = "product_format")]
            public Brand product_format { get; set; }

            [DataMember(Name = "value")]
            public string value { get; set; }


            [DataContract]
            public class Brand
            {
                [DataMember(Name = "id")]
                public int id { get; set; }

                [DataMember(Name = "status")]
                public string status { get; set; }

                [DataMember(Name = "created_by")]
                public int created_by { get; set; }

                [DataMember(Name = "created_on")]
                public DateTime created_on { get; set; }

                [DataMember(Name = "modified_by")]
                public int modified_by { get; set; }

                [DataMember(Name = "modified_on")]
                public DateTime modified_on { get; set; }

                [DataMember(Name = "name")]
                public string name { get; set; }

                [DataMember(Name = "value")]
                public string value { get; set; }
            }
        }
    }
}
