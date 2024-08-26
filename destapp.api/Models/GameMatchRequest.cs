using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models
{
    public class GameMatchRequest
    {
        public int idJuego { get; set; }
        public string idUsuario { get; set; }
        public int cantCoins { get; set; }
    }
}
