using destapp.api.Models.Request;
using destapp.api.Models.Response;
using destapp.apiClient;
using destapp.apiClient.CoreApiClient;
using destapp.biz.Entities;
using destapp.dal.db_context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageFirebase : ApiClient
    {
        //Send Notifications
        public static async Task<PushNotificationResponse> SendPushNotification(Notification notification, string to)
        {
            ApiClient apiClient = new ApiClient();
            var pushNotificationResponse = new PushNotificationResponse();
            PushNotificationRequest pushNotificationRequest = new PushNotificationRequest();
            pushNotificationRequest.to = to;
            pushNotificationRequest.notification.title = notification.Title;
            pushNotificationRequest.notification.text = notification.Text;
            pushNotificationRequest.data.id_notificacion = notification.Id;
            pushNotificationRequest.data.link = notification.DivLink;
            //Esto hace que se envíe la push notification
            apiClient.clearHeaders();
            Uri uri = apiClient.CreateRequestUri(Constants.FCM_SEND, relativePathBase: Constants.API_URL_BASE_FCM);
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("Authorization", Constants.SERVER_KEY);
            apiClient.setHeaders(headers);
            pushNotificationResponse = await apiClient.PostAsync<PushNotificationResponse, PushNotificationRequest>(uri, pushNotificationRequest);
            //fin
            return pushNotificationResponse;
        }
    }
}
