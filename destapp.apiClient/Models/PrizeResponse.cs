using System;
using System.Collections.Generic;
using System.Text;
using static destapp.apiClient.Models.AvatarResponse;
using static destapp.apiClient.Models.CataloguePrizeResponse;
using static destapp.apiClient.Models.HomeResponse;

namespace destapp.apiClient.Models
{
    public class PrizeResponse
    {

        public List<PrizeDatum> data { get; set; }

        public class FeaturedPrize_cms
        {
            public PrizeId_cms prize_id { get; set; }
        }

        public class nData
        {
            public PrizeId_cms data { get; set; }
        }


        public class PrizeId_cms
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public Image_pz image { get; set; }
            public Image_pz featured_image_web { get; set; }
            public Logo logo { get; set; }
            public string description { get; set; }
            public int stock { get; set; }
            public int prizes_per_day { get; set; }
            public int? tickets { get; set; }
            public string start_date { get; set; }
            public string finish_date { get; set; }
            public string users_total { get; set; }
            public string users_participations { get; set; }
            public string total_by_category { get; set; }
            public Category_pz category { get; set; }
            public bool featured { get; set; }
            public string name { get; set; }
            public int stock_used { get; set; }
            public object inactive_description { get; set; }
            public object inactive_days { get; set; }
            public object inactive_start_time { get; set; }
            public object inactive_end_time { get; set; }
            public object inactive_image { get; set; }
            public Notification_pz notification { get; set; }
           // public FeaturedImageWeb featured_image_web { get; set; }
            public object featured_image { get; set; }
            public bool me_interesa { get; set; }
            public int dias_habilitado { get; set; }
        }


        public class Notification_pz
        {
            public int id { get; set; }
        }

        public class Image_pz
        {
            public Data_pz data { get; set; }
        }

        public class Data_pz
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail_pz> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class Thumbnail_pz
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }


        public class PrizeId_pz
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy2 created_by { get; set; }
            public string created_on { get; set; }
            public CreatedBy2 modified_by { get; set; }
            public string modified_on { get; set; }
            public Image2_pz image { get; set; }
            public Logo2_pz logo { get; set; }
            public string description { get; set; }
            public int? stock { get; set; }
            public int? prizes_per_day { get; set; }
            public int? tickets { get; set; }
            public string start_date { get; set; }
            public string finish_date { get; set; }
            public string users_total { get; set; }
            public string users_participations { get; set; }
            public string total_by_category { get; set; }
            public Category_pz category { get; set; }
            public bool featured { get; set; }
            public string name { get; set; }
            public int? stock_used { get; set; }
            public object inactive_description { get; set; }
            public object inactive_days { get; set; }
            public object inactive_start_time { get; set; }
            public object inactive_end_time { get; set; }
            public object inactive_image { get; set; }
            public Notification2_pz notification { get; set; }
            public FeaturedImageWeb2_pz featured_image_web { get; set; }
            public object featured_image { get; set; }
            public bool me_interesa { get; set; }
            public int dias_habilitado { get; set; }
            public string fecha_canjeo { get; set; }
            public bool? habilitado { get; set; }
        }

        public class Thumbnail3_pz
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Data3_pz
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail3_pz> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class Image2_pz
        {
            public Data3_pz data { get; set; }
        }

        public class FeaturedImageWeb2_pz
        {
            public int id { get; set; }
        }

        public class Notification2_pz
        {
            public int id { get; set; }
        }

        public class Category_pz
        {
            public int id { get; set; }
        }

        public class Logo2_pz
        {
            public Data4_pz data { get; set; }
        }

        public class Data4_pz
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail4_pz> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class Thumbnail4_pz
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Prize_pz
        {
            public PrizeId_pz prize_id { get; set; }
        }


        public class PrizeDatum
        {
            public List<FeaturedPrize_cms> featured_prize { get; set; }
            public List<Prize_pz> prizes { get; set; }
            public string fecha_canjeo { get; set; }
            public int dias_habilitado { get; set; }
        }
    }
}
