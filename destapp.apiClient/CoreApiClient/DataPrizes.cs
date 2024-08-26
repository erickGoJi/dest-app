using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace destapp.apiClient.CoreApiClient
{
    public class DataPrizes: ApiClient
    {
        public DataPrizes() { }

        public async Task<PrizeResponse> GetAllPrizes()
        {
            clearHeaders();
          //  AuthenticationResponse authenticationResponse = await Authenticate();
           // Uri uri = CreateRequestUri("_/items/prizes_page", "fields=featured_prize.prize_id.*,featured_prize.prize_id.image.data.*,featured_prize.prize_id.featured_image.data.*,featured_prize.prize_id.logo.data.*,prizes.prize_id.*,prizes.prize_id.image.data.*,prizes.prize_id.logo.data.*");
            Uri uri = CreateRequestUri("_/items/prizes_page", "fields=featured_prize.prize_id.*,featured_prize.prize_id.image.data.*,featured_prize.prize_id.featured_image.data.*,featured_prize.prize_id.logo.data.*,featured_prize.prize_id.featured_image_web.data.*,prizes.prize_id.*,prizes.prize_id.image.data.*,prizes.prize_id.logo.data.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<PrizeResponse>(uri);
        }
    }
}
