using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static destapp.apiClient.Models.PrizeResponse;

namespace destapp.apiClient.Models.Trivia
{
    [DataContract]
    public class Trivia
    {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "image")]
        public Url image { get; set; }

        [DataMember(Name = "description")]
        public string description { get; set; }

        [DataMember(Name = "you_win_message")]
        public string you_win_message { get; set; }

        [DataMember(Name = "winner_message")]
        public string winner_message { get; set; }

        [DataMember(Name = "retry_message")]
        public string retry_message { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime start_date { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime end_date { get; set; }

        [DataMember(Name = "terms_and_conditions")]
        public string terms_and_conditions { get; set; }

        [DataMember(Name = "instructions_prize")]
        public Url instructions_prize { get; set; }

        [DataMember(Name = "inactive_image")]
        public Url inactive_image { get; set; }

        [DataMember(Name = "description_inactive")]
        public string description_inactive { get; set; }

        [DataMember(Name = "logo")]
        public Url logo { get; set; }

        [DataMember(Name = "start_inactive_time")]
        public string start_inactive_time { get; set; }

        [DataMember(Name = "end_inactive_time")]
        public string end_inactive_time { get; set; }

        [DataMember(Name = "inactive_days")]
        public string[] inactive_days { get; set; }

        [DataMember(Name = "instructions")]
        public string instructions { get; set; }

        [DataMember(Name = "amount")]
        public int? amount { get; set; }

        [DataMember(Name = "prize_per_user")]
        public int? prize_per_user { get; set; }

        [DataMember(Name = "times_per_user")]
        public int? times_per_user { get; set; }

        [DataMember(Name = "access_coins")]
        public int? access_coins { get; set; }
        
        [DataMember(Name = "access_type")]
        public AccessType access_type { get; set; }

        [DataMember(Name = "featured_image_web")]
        public Url featured_image_web { get; set; }

        [DataMember(Name = "featured_image")]
        public Url featured_image { get; set; }

        [DataMember(Name = "access_codes")]
        public List<AccessCode> access_codes { get; set; }

        [DataMember(Name = "age_range")]
        public List<Age> age_range { get; set; }

        [DataMember(Name = "gender")]
        public List<Gender> gender { get; set; }

        [DataMember(Name = "interests")]
        public List<Interest> interests { get; set; }

        [DataMember(Name = "notification")]
        public List<NotificationDatum> notification { get; set; }

        [DataMember(Name = "so")]
        public List<SO> so { get; set; }

        [DataMember(Name = "states")]
        public List<State> states { get; set; }

        [DataMember(Name = "prize")]
        public List<Prize_pz> prize { get; set; }

        [DataMember(Name = "questions")]
        public List<QuestionData> questions { get; set; }

        [DataMember(Name = "prize_type")]
        public string prize_type { get; set; }

        [DataMember(Name = "day_disp")]
        public int? day_disp { get; set; } = 0;

        [DataMember(Name = "prize_per_day")]
        public int? prize_per_day { get; set; } = 0;

        [DataMember(Name = "intentos_disp")]
        public int? intentos_disp { get; set; } = 0;

        [DataMember(Name = "habilitado")]
        public bool? habilitado { get; set; } = false;

        [DataMember(Name = "me_interesa")]
        public bool me_interesa { get; set; } = false;

        public class QuestionData
        {
            public Question question_id { get; set; }
        }
    }
}
