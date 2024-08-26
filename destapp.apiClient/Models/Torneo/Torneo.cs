using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static destapp.apiClient.Models.AccessCode;
using static destapp.apiClient.Models.HomeResponse;
using static destapp.apiClient.Models.PrizeResponse;

namespace destapp.apiClient.Models.Torneo
{
    [DataContract]
    public class Torneo
    {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "created_by")]
        public User created_by { get; set; }

        [DataMember(Name ="created_on")]
        public DateTime created_on { get; set; }

        [DataMember(Name = "modified_by")]
        public User modified_by { get; set; }

        [DataMember(Name = "modified_on")]
        public DateTime modified_on { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "image")]
        public Url image { get; set; }

        [DataMember(Name = "description")]
        public string description { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime start_date { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime end_date { get; set; }

        [DataMember(Name = "time_length")]
        public int? time_length { get; set; }

        [DataMember(Name = "game")]
        public shortGameData game { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "template_email")]
        public string template_email { get; set; }

        [DataMember(Name = "instructions")]
        public string instructions { get; set; }

        [DataMember(Name = "instructions_image")]
        public Url instructions_image { get; set; }

        [DataMember(Name = "terms_and_conditions")]
        public string terms_and_conditions { get; set; }

        [DataMember(Name = "access_type")]
        public BasicCatalogData access_type { get; set; }

        [DataMember(Name = "amount")]
        public int? amount { get; set; }

        [DataMember(Name = "codes")]
        public List<shortCodesData> codes { get; set; }

        [DataMember(Name = "prize")]
        public List<Prize_pz> prize { get; set; }

        [DataMember(Name = "day_disp")]
        public int? day_disp { get; set; } = 0;

        [DataMember(Name = "intentos_disp")]
        public int? intentos_disp { get; set; } = 0;

        [DataContract]
        public class shortCodesData
        {
            [DataMember(Name = "codes_id")]
            public Code codes_id { get; set; }

            [DataContract]
            public class Code
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

                [DataMember(Name = "brand")]
                public BrandT brand { get; set; }

                [DataMember(Name = "product_format")]
                public BrandT product_format { get; set; }

                [DataMember(Name = "value")]
                public string value { get; set; }

                [DataContract]
                public class BrandT
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

                    [DataMember(Name = "value")]
                    public string value { get; set; }
                }
            }
        }

        [DataContract]
        public class shortGameData
        {
            [DataMember(Name = "id")]
            public int id { get; set; }

            [DataMember(Name = "status")]
            public string status { get; set; }

            [DataMember(Name = "created_by")]
            public int? created_by { get; set; }

            [DataMember(Name = "created_on")]
            public DateTime created_on { get; set; }

            [DataMember(Name = "modified_by")]
            public int? modified_by { get; set; }

            [DataMember(Name = "modified_on")]
            public DateTime modified_on { get; set; }

            [DataMember(Name = "title")]
            public string title { get; set; }

            [DataMember(Name = "image")]
            public int? image { get; set; }

            [DataMember(Name = "start_date")]
            public DateTime start_date { get; set; }

            [DataMember(Name = "end_date")]
            public DateTime end_date { get; set; }

            [DataMember(Name = "coins")]
            public int? coins { get; set; }

            [DataMember(Name = "featured_image")]
            public int? featured_image { get; set; }

            [DataMember(Name = "description")]
            public string description { get; set; }

            [DataMember(Name = "tickets")]
            public int? tickets { get; set; }

            [DataMember(Name = "access_coins")]
            public int? access_coins { get; set; }

            [DataMember(Name = "terms_and_conditions")]
            public string terms_and_conditions { get; set; }

            [DataMember(Name = "promo")]
            public int? promo { get; set; }

            [DataMember(Name = "url_game")]
            public string url_game { get; set; }

            [DataMember(Name = "access_type")]
            public int? access_type { get; set; }

            [DataMember(Name = "featured_image_web")]
            public int? featured_image_web { get; set; }

        }

    }
}
