using destapp.apiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Juegos
{
    public class JuegoWBiggerScore : Game
    {
        public BiggerScore biggerScore {get;set;}
    }
}
