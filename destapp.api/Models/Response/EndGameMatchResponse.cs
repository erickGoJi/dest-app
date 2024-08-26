using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class EndGameMatchResponse
    {
        public int idBiggerScore { get; set; }
        public int biggerScore { get; set; }
        public int ticketsWon { get; set; }
        public int score { get; set; }
        public string idUsuario { get; set; }
        public int idPartida { get; set; }
        public int idJuego { get; set; }
    }
}
