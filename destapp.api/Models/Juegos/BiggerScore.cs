using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Juegos
{
    public class BiggerScore
    {
        public int idJuego { get; set; }
        public int idPartida { get; set; }
        public int idUsuario { get; set; }
        public int idCms { get; set; }
        public int? Score { get; set; }
    }
}
