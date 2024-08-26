using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models.Trivia
{
    public class TriviaResponse
    {
        public List<DataTrivia> data { get; set; }
        // Se cambió de Data a DataGame por problemas de ejecución
        public class DataTrivia
        {
            public List<FeaturedTrivia> featured_trivia { get; set; }
            public List<TriviaPrivate> trivia { get; set; }
        }

        public class TriviaPrivate
        {
            public Trivia trivia_id { get; set; }
        }

        public class FeaturedTrivia
        {
            public Trivia featured_trivia_id { get; set; }
        }
    }
}
