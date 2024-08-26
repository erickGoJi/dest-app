using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class DataAvatar: ApiClient
    {
        public DataAvatar() { }

        public async Task<AvatarResponse> GetAllAvatar()
        {
            clearHeaders();
           // AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("_/items/avatar", "fields=*,image.data.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<AvatarResponse>(uri);
        }
    }
}
