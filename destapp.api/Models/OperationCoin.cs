using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models
{
    public class OperationCoin
    {
        public string idUser { get; set; }
        public int? type { get; set; }
        public int? source { get; set; }
        public int? coins { get; set; }
    }
}
