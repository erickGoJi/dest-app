using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Request
{
    public class IniciarTorneoRequest
    {
        public int idJuego { get; set; }
        public string idUsuario { get; set; }
        public int cantCoins { get; set; }
        public int idTorneo { get; set; }
        public int torneo_type { get; set; }
        public int access_type { get; set; }
    }
}
