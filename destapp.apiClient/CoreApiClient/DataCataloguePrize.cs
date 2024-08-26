using destapp.apiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    class DataCataloguePrize : ApiClient
    {
        public DataCataloguePrize()
        {
        }

        public async Task<CataloguePrizeResponse> GetAllBadges()
        {
            clearHeaders();
           // AuthenticationResponse authenticationResponse = await Authenticate();
            Uri uri = CreateRequestUri("_/items/prizes_page", "fields=featured_prize.*,featured_prize.image.data.*,featured_prize.logo.data.*,prizes.prize_id.*,prizes.prize_id.image.data.*,prizes.prize_id.logo.data.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<CataloguePrizeResponse>(uri);
        }
    }
}
