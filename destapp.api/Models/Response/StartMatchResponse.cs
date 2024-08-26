using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class StartMatchResponse
    {
        public int idPartida { get; set; }
        public string idUsuario { get; set; }
        public string avatar { get; set; }
        public string userName { get; set; }
        public int idCoinsUser { get; set; }
        public int idCoinsUserHistory { get; set; }
        public int coinsUser { get; set; }
    }
}
