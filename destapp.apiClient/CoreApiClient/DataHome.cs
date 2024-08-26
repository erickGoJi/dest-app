using destapp.apiClient.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace destapp.apiClient.CoreApiClient
{
    public class DataHome: ApiClient
    {
        public DataHome() { }

        public async Task<HomeResponse> GetAllHome()
        {
            clearHeaders();
            //  AuthenticationResponse authenticationResponse = await Authenticate(); https://cms.destapp.com/_/items/home?fields=featured_content.featured_content_id.*,featured_content.featured_content_id.image.data.full_url.*,featured_content.featured_content_id.action_type.*,featured_content.featured_content_id.section.*,featured_content.featured_content_id.game.*,featured_content.featured_content_id.url.*,featured_content.featured_content_id.trivia.*.*,featured_content.featured_content_id.prizes.*.*,featured_content.featured_content_id.image_web.data.*,prizes.prize.*,prizes.prize.image.data.*,prizes.prize.logo.data.*,prizes_two.prize.*,prizes_two.prize.image.data.*,prizes_two.prize.logo.data.*
            Uri uri = CreateRequestUri("_/items/home", "fields=featured_content.featured_content_id.*,featured_content.featured_content_id.image.data.full_url.*,featured_content.featured_content_id.action_type.*,featured_content.featured_content_id.section.*,featured_content.featured_content_id.game.*,featured_content.featured_content_id.url.*,featured_content.featured_content_id.trivia.*.*,featured_content.featured_content_id.prizes.*.*,featured_content.featured_content_id.image_web.data.*,prizes.prize.*,prizes.prize.image.data.*,prizes.prize.logo.data.*,prizes_two.prize.*,prizes_two.prize.image.data.*,prizes_two.prize.logo.data.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<HomeResponse>(uri);
        }
    }
}
