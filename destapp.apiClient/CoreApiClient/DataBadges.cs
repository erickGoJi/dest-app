using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class DataBadges : ApiClient
    {

        public DataBadges()
        {
        }

        public async Task<BadgesResponse> GetAllBadges()
        {
            clearHeaders();
          //  AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("_/items/badges", "fields=*,image.data.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<BadgesResponse>(uri);
        }
    }
}
