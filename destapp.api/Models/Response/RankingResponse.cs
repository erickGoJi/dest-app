using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class RankingResponse
    {
        public RankingDatum dataData = new RankingDatum();
        public List<RankingDatum> listData = new List<RankingDatum>();
        /*public RankingData data = new RankingData();
        public class RankingData
        {
            public RankingDatum dataData = new RankingDatum();
            public List<RankingDatum> listData = new List<RankingDatum>();
        }*/

        public class RankingDatum
        {
            public string idUser { get; set; }
            public int? idAvatar { get; set; }
            public string fullUrlAvatar { get; set; }
            public int userScore { get; set; }
            public string gameName { get; set; }
            public string nameUser { get; set; }
            public int position { get; set; }
        }
    }
}
