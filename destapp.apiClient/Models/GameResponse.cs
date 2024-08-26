using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class GameResponse
    {
        public List<DataGame> data { get; set; }
        // Se cambió de Data a DataGame por problemas de ejecución
        public class DataGame
        {
            public Game featured_game { get; set; }
            public List<GamePrivate> games { get; set; }
        }

        public class GamePrivate
        {
            public Game game_id { get; set; }
        }
    }
}
