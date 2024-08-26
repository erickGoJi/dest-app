using System;
using System.Collections.Generic;
using System.Text;
using static destapp.apiClient.Models.AvatarResponse;

namespace destapp.apiClient.Models
{
    public class CataloguePrizeResponse
    {
        List<CataloguePrizeDatum> data { get; set; }

        public class CataloguePrizeDatum
        {
            public FeaturedPrize featured_prize { get; set; }
            public List<Prize> prizes { get; set; }
        }

        public class FeaturedPrize
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public Url image { get; set; }
            public Logo logo { get; set; }
            public string description { get; set; }
            public int stock { get; set; }
            public int prizes_per_day { get; set; }
            public int tickets { get; set; }
            public string start_date { get; set; }
            public string finish_date { get; set; }
            public string users_total { get; set; }
            public string users_participations { get; set; }
            public string total_by_category { get; set; }
            public Category category { get; set; }
            public bool featured { get; set; }
            public string name { get; set; }
            public object stock_used { get; set; }
            public object inactive_description { get; set; }
            public object inactive_days { get; set; }
            public object inactive_start_time { get; set; }
            public object inactive_end_time { get; set; }
            public object inactive_image { get; set; }
            public NotificationApiClient notification { get; set; }
        }

        //public class Logo
        //{
        //    public Url data { get; set; }
        //}

        public class Thumbnail5
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Data5
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail5> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class Logo
        {
            public Data5 data { get; set; }
        }

        public class Category
        {
            public int id { get; set; }
        }

        public class NotificationApiClient
        {
            public int id { get; set; }
        }

        public class Prize
        {
            public FeaturedPrize prize_id { get; set; }
        }
    }
}
