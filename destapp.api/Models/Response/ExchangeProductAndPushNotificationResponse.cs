using destapp.biz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class ExchangeProductAndPushNotificationResponse
    {
        public ExchangeProductHistory exchangeProductHistory { get; set; }
        public PushNotificationResponse pushNotificationResponse { get; set; }
        public Notification notification { get; set; }

        public ExchangeProductAndPushNotificationResponse()
        {
            exchangeProductHistory = new ExchangeProductHistory();
            pushNotificationResponse = new PushNotificationResponse();
            notification = new Notification();
        }
    }
}
