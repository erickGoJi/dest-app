using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Request
{
    public class EndGameMatchRequest
    {
        public int score { get; set; }
        public int? promo { get; set; }
        public string idUsuario { get; set; }
        public int idPartida { get; set; }
    }
}
