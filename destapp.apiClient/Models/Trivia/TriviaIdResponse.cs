using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models.Trivia
{
    public class TriviaIdResponse
    {
        //public Data data { get; set; }
        //// Se cambió de Data a DataGame por problemas de ejecución....
        ////public class DataTrivia
        ////{
        ////    public TriviaPrivate trivia { get; set; }
        ////}

        //public class Data
        //{
        //    public Trivia trivia_id { get; set; }
        //}

        public id_Data data { get; set; }

        public class id_Thumbnail
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class id_Data2
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class id_Image
        {
            public id_Data2 data { get; set; }
        }

        public class id_Thumbnail2
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class id_Data3
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<id_Thumbnail2> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class id_InstructionsPrize
        {
            public id_Data3 data { get; set; }
        }

        public class id_Thumbnail3
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class id_Data4
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<id_Thumbnail3> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class id_Logo
        {
            public id_Data4 data { get; set; }
        }

        public class id_AccessType
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

        public class id_CreatedBy
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class id_ModifiedBy
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class id_Brand
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
            public object logo { get; set; }
        }

        public class id_ProductFormat
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

        public class id_CodeId
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public id_CreatedBy created_by { get; set; }
            public string created_on { get; set; }
            public id_ModifiedBy modified_by { get; set; }
            public string modified_on { get; set; }
            public string name { get; set; }
            public List<string> type { get; set; }
            public int coins_to_add { get; set; }
            public object tickets_to_add { get; set; }
            public object image { get; set; }
            public id_Brand brand { get; set; }
            public id_ProductFormat product_format { get; set; }
            public string value { get; set; }
        }

        public class id_AccessCode
        {
            public id_CodeId code_id { get; set; }
        }

        public class id_CreatedBy2
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
            public List<object> roles { get; set; }
        }

        public class id_ModifiedBy2
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
            public List<object> roles { get; set; }
        }

        public class id_CreatedBy3
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class id_ModifiedBy3
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public object external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class id_QuestionId2
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string question { get; set; }
            public object image { get; set; }
            public object image_type { get; set; }
            public object type { get; set; }
        }

        public class id_AnswerId
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string answer { get; set; }
            public bool correct_answer { get; set; }
        }

        public class id_Answer
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public id_CreatedBy3 created_by { get; set; }
            public string created_on { get; set; }
            public id_ModifiedBy3 modified_by { get; set; }
            public string modified_on { get; set; }
            public id_QuestionId2 question_id { get; set; }
            public id_AnswerId answer_id { get; set; }
        }

        public class id_QuestionId
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public id_CreatedBy2 created_by { get; set; }
            public string created_on { get; set; }
            public id_ModifiedBy2 modified_by { get; set; }
            public string modified_on { get; set; }
            public string question { get; set; }
            public object image { get; set; }
            public object image_type { get; set; }
            public object type { get; set; }
            public List<id_Answer> answers { get; set; }
        }

        public class id_Question
        {
            public id_QuestionId question_id { get; set; }
        }

        public class id_Data
        {
            public int? id { get; set; }
            public string name { get; set; }
            public id_Image image { get; set; }
            public string description { get; set; }
            public string you_win_message { get; set; }
            public string winner_message { get; set; }
            public string retry_message { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string terms_and_conditions { get; set; }
            public id_InstructionsPrize instructions_prize { get; set; }
            public object inactive_image { get; set; }
            public string description_inactive { get; set; }
            public id_Logo logo { get; set; }
            public object start_inactive_time { get; set; }
            public object end_inactive_time { get; set; }
            public List<object> inactive_days { get; set; }
            public string instructions { get; set; }
            public string prize_type { get; set; }
            public int? amount { get; set; }
            public int? prize_per_user { get; set; }
            public int? times_per_user { get; set; }
            public int? access_coins { get; set; }
            public AccessType access_type { get; set; }
            public List<AccessCode> access_codes { get; set; }
            public List<object> age_range { get; set; }
            public List<object> gender { get; set; }
            public List<object> interests { get; set; }
            public List<object> notification { get; set; }
            public List<Prize_id> prize { get; set; }
            public List<id_Question> questions { get; set; }
            public List<object> so { get; set; }
            public List<object> states { get; set; }
        }

        public class Prize_id
        {
            public PrizeId_id prize_id { get; set; }
        }

        public class PrizeId_id
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public CreatedBy_ created_by { get; set; }
            public string created_on { get; set; }
            public ModifiedBy_ modified_by { get; set; }
            public string modified_on { get; set; }
            public Image2_ image { get; set; }
            public Logo2_ logo { get; set; }
            public string description { get; set; }
            public int stock { get; set; }
            public int prizes_per_day { get; set; }
            public int tickets { get; set; }
            public string start_date { get; set; }
            public string finish_date { get; set; }
            public string users_total { get; set; }
            public string users_participations { get; set; }
            public string total_by_category { get; set; }
            public Category_ category { get; set; }
            public string name { get; set; }
            public int stock_used { get; set; }
            public object inactive_description { get; set; }
            public object inactive_days { get; set; }
            public object inactive_start_time { get; set; }
            public object inactive_end_time { get; set; }
            public object inactive_image { get; set; }
            public Notification2_ notification { get; set; }
            public object featured_image_web { get; set; }
            public object featured_image { get; set; }
            public object type { get; set; }
        }


        public class Logo2_
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
            public Data8_ data { get; set; }
        }

        public class Category_
        {
            public int id { get; set; }
            public string status { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public string name { get; set; }
            public string value { get; set; }
            public int? prices_per_month { get; set; }
            public int? prices_per_year { get; set; }
        }

        public class Notification2_
        {
            public int id { get; set; }
            public string status { get; set; }
            public object sort { get; set; }
            public int created_by { get; set; }
            public string created_on { get; set; }
            public int modified_by { get; set; }
            public string modified_on { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public int image { get; set; }
            public string notification_type { get; set; }
            public int icon { get; set; }
            public object deeplink_app { get; set; }
            public object deeplink_web { get; set; }
        }

        public class CreatedBy_
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
            public List<object> roles { get; set; }
        }

        public class ModifiedBy_
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
            public List<object> roles { get; set; }
        }

        public class UploadedBy_
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class Thumbnail7_
        {
            public string url { get; set; }
            public string relative_url { get; set; }
            public string dimension { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Data8_
        {
            public string full_url { get; set; }
            public string url { get; set; }
            public List<Thumbnail7_> thumbnails { get; set; }
            public object embed { get; set; }
        }

        public class Image2_
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
            public Data8_ data { get; set; }
        }

        public class CreatedBy2_
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

        public class ModifiedBy3_
        {
            public int id { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string timezone { get; set; }
            public string locale { get; set; }
            public object locale_options { get; set; }
            public object avatar { get; set; }
            public object company { get; set; }
            public object title { get; set; }
            public string external_id { get; set; }
            public object __invalid_name__2fa_secret { get; set; }
        }

    }
}
