using System;

namespace destapp.api.Models.Request
{
    public class PushNotificationRequest
    {
        public string to { get; set; }
        public PNotification notification { get; set; }
        public DataNotification data { get; set; }

        public class PNotification
        {
            public string title { get; set; }
            public string text { get; set; }
        }

        public class DataNotification
        {
            public long id_notificacion { get; set; }
			public string link { get; set; }
        }

        public PushNotificationRequest()
        {
            notification = new PNotification();
            data = new DataNotification();
        }
    }
}
