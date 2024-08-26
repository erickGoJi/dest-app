using destapp.apiClient.Models.Torneo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class Torneo : ApiClient
    {
        public Torneo()
        {

        }

        public async Task<TorneoResponse> getAllTorneos()
        {
            clearHeaders();
            // AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("/_/items/tournaments", "filter[status]=published&fields=*,image.data.*,game.*,instructions_image.data.*,codes.codes_id.*.*.*,prize.prize_id.*.*,access_type.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<TorneoResponse>(uri);
        }
    }
}
