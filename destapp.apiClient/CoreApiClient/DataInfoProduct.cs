using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;
using static destapp.apiClient.Models.PrizeResponse;

namespace destapp.apiClient.CoreApiClient
{
    public class DataInfoProduct: ApiClient
    {
        public async Task<nData> GetNameProduct(int idProduct)
        {
            clearHeaders();
            Uri uri = CreateRequestUri(string.Format("_/items/prizes/{0}", idProduct), "fields=*,image.data.*,logo.data.*,category.*,notification.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<nData>(uri);
        }
    }
}
