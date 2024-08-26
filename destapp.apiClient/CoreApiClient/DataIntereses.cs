using destapp.apiClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class DataIntereses : ApiClient
    {
        public DataIntereses()
        {
        }

        public async Task<InteresesResponse> GetAllIntereses()
        {
            clearHeaders();
          //  AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("/_/items/interests");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<InteresesResponse>(uri);
        }
    }
}
