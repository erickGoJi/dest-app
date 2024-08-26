using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class Score
    {
        public int idJuego { get; set; }
        public int idPartida { get; set; }
        public String idUsuario { get; set; }
        public int idCms { get; set; }
        public int? score { get; set; }
    }
}
