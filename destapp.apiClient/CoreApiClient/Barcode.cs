using destapp.apiClient.Models;
using destapp.apiClient.Models.Trivia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class Barcode : ApiClient
    {
        public Barcode()
        {

        }
        
        public async Task<BarcodeResponse> get_barcode(string barcode)
        {
            clearHeaders();
            // AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("_/items/codes", "filter[value]=" + barcode +"&fields=*,brand.*,product_format.*,coins_to_add,tickets_to_add");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<BarcodeResponse>(uri);
        }
    }
}
