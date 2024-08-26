using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Request
{
    public class SaveTokenRequest
    {
        public string idUser { get; set; }
        public string token { get; set; }
    }
}
