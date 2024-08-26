using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static destapp.apiClient.Models.HomeResponse;

namespace destapp.apiClient.Models.Trivia
{
    [DataContract]
    public class Question
    {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "created_by")]
        public CreatedBy2 created_by { get; set; }
        
        [DataMember(Name = "created_on")]
        public DateTime created_on { get; set; }

        [DataMember(Name ="modified_by")]
        public CreatedBy2 modified_by { get; set; }

        [DataMember(Name = "modified_on")]
        public DateTime modified_on { get; set; }

        [DataMember(Name = "question")]
        public string question { get; set; }

        [DataMember(Name = "image")]
        public Url image { get; set; }

        [DataMember(Name = "image_type")]
        public string image_type { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name ="answers")]
        public List<Answer> answers { get; set; }

        public class Answer
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

            [DataMember(Name ="answer_id")]
            public AnswerData answer_id { get; set; }

        }

        public class AnswerData
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

            [DataMember(Name = "answer")]
            public string answer { get; set; }

            [DataMember(Name = "correct_answer")]
            public object correct_answer { get; set; }


        }


    }
}
