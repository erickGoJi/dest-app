using destapp.biz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Request
{
    public class ExchangeProductAndSendNotificationRequest
    {
        public ExchangeProductHistory exchangeProductHistory { get; set; }
        public DataSendNotification dataSendNotification { get; set; }

        public class DataSendNotification
        {
            public string deviceToken { get; set; }
            public string typeNotification { get; set; }
        }
    }
}
