using destapp.apiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class DiscountStock : ApiClient
    {
        public async static Task<DiscountStockResponse> DiscountStockApi(int idProduct)
        {
            DiscountStockResponse discount = new DiscountStockResponse();
            ApiClient apiClient = new ApiClient();
            apiClient.clearHeaders();
            Uri uri = apiClient.CreateRequestUri($"/stock/reduce/{idProduct}", relativePathBase: Constants.CONTROLLERS_DESTAPP);
            var response = await apiClient.PatchAsync(uri, discount);
            return response.success ? response : null;
        }

        public async static Task<DiscountStockResponse> DiscountStockApiTrivia(int idProduct, int idTrivia)
        {
            DiscountStockResponse discount = new DiscountStockResponse();
            ApiClient apiClient = new ApiClient();
            apiClient.clearHeaders();
            Uri uri = apiClient.CreateRequestUri($"/stock/reduce/{idProduct}/{idTrivia}", relativePathBase: Constants.CONTROLLERS_DESTAPP);
            var response = await apiClient.PatchAsync(uri, discount);
            return response.success ? response : null;
        }
    }
}
