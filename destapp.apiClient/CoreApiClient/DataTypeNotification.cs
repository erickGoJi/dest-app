using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class DataTypeNotification : ApiClient
    {
        public DataTypeNotification()
        {
        }

        public async Task<TypeNotificationResponse> GetTypeNotificationByName(string typeNotification)
        {
            clearHeaders();
            Uri uri = CreateRequestUri("_/items/automatic_notifications", string.Format("{0}", "filter[notification_type]="+typeNotification+"&fields=*,image.data.*,icon.data.*"), null);
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<TypeNotificationResponse>(uri);
        }


    }
}
