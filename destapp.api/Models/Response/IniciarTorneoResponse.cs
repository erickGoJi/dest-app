using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class IniciarTorneoResponse
    {
        public int idPartida { get; set; }
        public string idUsuario { get; set; }
        public string userName { get; set; }
        public string avatar { get; set; }
    }
}
