using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static destapp.apiClient.Models.AvatarResponse;
using static destapp.apiClient.Models.CataloguePrizeResponse;

namespace destapp.apiClient.Models
{
    public class HomeResponse
    {
        public List<HomeDatum> data { get; set; }

        public class ActionType
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Section
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Game2
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string title { get; set; }
            public int image { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public int coins { get; set; }
            public int featured_image { get; set; }
            public string description { get; set; }
            public int tickets { get; set; }
            public int access_coins { get; set; }
            public string terms_and_conditions { get; set; }
            public int promo { get; set; }
            public string url_game { get; set; }
            public int access_type { get; set; }
            public object featured_image_web { get; set; }
        }
        [DataContract]
        public class CreatedBy2
        {
            [DataMember(Name = "id")]
            public int id { get; set; }
            [DataMember(Name = "status")]
            public string status { get; set; }
            [DataMember(Name = "first_name")]
            public string first_name { get; set; }
            [DataMember(Name = "last_name")]
            public string last_name { get; set; }
            [DataMember(Name = "email")]
            public string email { get; set; }
            [DataMember(Name = "token")]
            public object token { get; set; }
            [DataMember(Name = "time_zone")]
            public string timezone { get; set; }
            [DataMember(Name = "locale")]
            public string locale { get; set; }
            [DataMember(Name = "locale_options")]
            public object locale_options { get; set; }
            [DataMember(Name = "avatar")]
            public object avatar { get; set; }
            [DataMember(Name = "company")]
            public object company { get; set; }
            [DataMember(Name = "title")]
            public object title { get; set; }
            [DataMember(Name = "email_notifications")]
            public bool email_notifications { get; set; }
            [DataMember(Name = "last_acces_on")]
            public DateTime last_access_on { get; set; }
            [DataMember(Name = "last_page")]
            public string last_page { get; set; }
            [DataMember(Name = "external_id")]
            public string external_id { get; set; }
            [DataMember(Name ="2fa_secret")]
            public object fa_secret { get; set; }
        }

        public class ModifiedBy2
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public object token { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public bool email_notifications { get; set; }
            public DateTime last_access_on { get; set; }
            public string last_page { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class Image2
        {
            public int id { get; set; }
            public string storage { get; set; }
            public string filename { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public int uploaded_by { get; set; }
            public DateTime uploaded_on { get; set; }
            public string charset { get; set; }
            public int filesize { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int duration { get; set; }
            public object embed { get; set; }
            public object folder { get; set; }
            public string description { get; set; }
            public string location { get; set; }
            public List<object> tags { get; set; }
            public string checksum { get; set; }
            public object metadata { get; set; }
            public Url data { get; set; }
        }

        public class InactiveImage
        {
            public int id { get; set; }
            public string storage { get; set; }
            public string filename { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public int uploaded_by { get; set; }
            public DateTime uploaded_on { get; set; }
            public string charset { get; set; }
            public int filesize { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int duration { get; set; }
            public object embed { get; set; }
            public object folder { get; set; }
            public string description { get; set; }
            public string location { get; set; }
            public List<object> tags { get; set; }
            public string checksum { get; set; }
            public object metadata { get; set; }
            public Url data { get; set; }
        }

        public class AccessType2
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }

        public class TriviaHome
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy2 created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy2 modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public Image2 image { get; set; }
            public string description { get; set; }
            public string you_win_message { get; set; }
            public string winner_message { get; set; }
            public string retry_message { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string terms_and_conditions { get; set; }
            public object instructions_prize { get; set; }
            public InactiveImage inactive_image { get; set; }
            public string description_inactive { get; set; }
            public object logo { get; set; }
            public object start_inactive_time { get; set; }
            public object end_inactive_time { get; set; }
            public List<object> inactive_days { get; set; }
            public string instructions { get; set; }
            public string prize_type { get; set; }
            public object amount { get; set; }
            public int prize_per_day { get; set; }
            public int prize_per_user { get; set; }
            public int times_per_user { get; set; }
            public int access_coins { get; set; }
            public AccessType2 access_type { get; set; }
            public object featured_image_web { get; set; }
            public object featured_image { get; set; }
            public List<object> access_codes { get; set; }
            public List<object> age_range { get; set; }
            public List<object> gender { get; set; }
            public List<object> interests { get; set; }
            public List<object> notification { get; set; }
            public List<object> prize { get; set; }
            public List<object> questions { get; set; }
            public List<object> so { get; set; }
            public List<object> states { get; set; }
        }

        public class FeaturedContentId
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public string title { get; set; }
            public Url image { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public ActionType action_type { get; set; }
            public Section section { get; set; }
            public Game2 game { get; set; }
            public int? coins { get; set; }
            public int? tickets { get; set; }
            public TriviaHome trivia { get; set; }
            public object tournament { get; set; }
            public string link { get; set; }
            public object url { get; set; }
            public object image_web { get; set; }
            public List<object> age_range { get; set; }
            public List<object> codes { get; set; }
            public List<object> gender { get; set; }
            public List<object> interests { get; set; }
            public List<object> prizes { get; set; }
            public List<object> so { get; set; }
            public List<object> states { get; set; }
        }

        public class FeaturedContent
        {
            public FeaturedContentId featured_content_id { get; set; }
        }

        public class Category
        {
            public int id { get; set; }
        }

        public class FeaturedImageWeb
        {
            public int id { get; set; }
        }

        public class Prize2
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
            public int? tickets { get; set; }
            public string start_date { get; set; }
            public string finish_date { get; set; }
            public string users_total { get; set; }
            public string users_participations { get; set; }
            public string total_by_category { get; set; }
            public Category category { get; set; }
            public bool featured { get; set; }
            public string name { get; set; }
            public int stock_used { get; set; }
            public object inactive_description { get; set; }
            public object inactive_days { get; set; }
            public object inactive_start_time { get; set; }
            public object inactive_end_time { get; set; }
            public object inactive_image { get; set; }
            public NotificationApiClient notification { get; set; }
            public FeaturedImageWeb featured_image_web { get; set; }
            public object featured_image { get; set; }
            public bool me_interesa { get; set; }
            public int dias_habilitado { get; set; }
            public bool? habilitado { get; set; } = false;
        }

        public class Prize
        {
            public Prize2 prize { get; set; }
        }

        public class HomeDatum
        {
            public List<FeaturedContent> featured_content { get; set; }
            public List<Prize> prizes { get; set; }
            public List<Prize> prizes_two { get; set; }
            public string fecha_canjeo { get; set; }
            public int dias_habilitado { get; set; }
        }
    
    }
}
