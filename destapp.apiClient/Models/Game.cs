using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace destapp.apiClient.Models
{
    [DataContract]
    public class Game
    {
        [DataMember(Name = "idCatalog")]
        public int id { get; set; }
        [DataMember(Name = "id")]
        public int idCMS { get; set; }

        [DataMember(Name = "status")]
        public String status { get; set; }

        [DataMember(Name = "created_by")]
        public User created_by{ get; set; }

        [DataMember(Name = "created_on")]
        public DateTime created_on { get; set; }

        [DataMember(Name = "modified_by")]
        public User modified_by { get; set; }

        [DataMember(Name = "modified_on")]
        public DateTime modified_on { get; set; }

        [DataMember(Name = "title")]
        public String title { get; set; }

        [DataMember(Name = "image")]
        public Url image { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime start_date { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime end_date { get; set; }

        [DataMember(Name = "coins")]
        public int coins { get; set; }

        [DataMember(Name = "url_game")]
        public String url_game { get; set; }

        [DataMember(Name = "featured_image")]
        public Url featured_image { get; set; }

        [DataMember(Name = "featured_image_web")]
        public Url featured_image_web { get; set; }

        [DataMember(Name = "description")]
        public String description { get; set; }        

        [DataMember(Name = "tickets")]
        public String tickets { get; set; }

        [DataMember(Name = "access_coins")]
        public int? acccess_coins { get; set; }

        [DataMember(Name = "terms_and_conditions")]
        public String terms_and_conditions { get; set; }

        [DataMember(Name = "promo")]
        public Promo promo { get; set; }

        [DataMember(Name = "access_type")]
        public AccessType access_type { get; set; }

        [DataMember(Name = "access_codes")]
        public List<AccessCode> access_codes { get; set; }

        [DataMember(Name = "ages")]
        public List<Age> ages { get; set; }

        [DataMember(Name = "gender")]
        public List<Gender> gender { get; set; }

        [DataMember(Name = "interests")]
        public List<Interest> interests { get; set; }

        [DataMember(Name = "so")]
        public List<SO> so { get; set; }

        [DataMember(Name = "states")]
        public List<State> states { get; set; }
        
        [DataMember(Name ="biggerScore")]
        public Score biggerScore { get; set; }
    }
}
