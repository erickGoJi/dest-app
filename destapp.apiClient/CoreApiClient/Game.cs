using destapp.apiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public class Game : ApiClient
    {
        public Game()
        {

        }

        public async Task<GameResponse> getAllGames()
        {
            clearHeaders();
            // AuthenticationResponse authenticationResponse = await Authenticate();
            // Uri uri = CreateRequestUri("/_/items/games_page", "fields=featured_game.*,featured_game.image.data.*,featured_game.featured_image.data.*,games.game_id.*,games.game_id.image.data.*,games.game_id.featured_image.data.*,games.game_id.access_codes.*,games.game_id.ages.age_range_id.*,games.game_id.gender.gender_id.*,games.game_id.interests.interests_id.*,games.game_id.so.so_id.*,games.game_id.states.state_id.*,games.game_id.access_codes.codes_id.image.data.*,games.game_id.promo.*");
               Uri uri = CreateRequestUri("/_/items/games_page", "fields=featured_game.*,featured_game.image.data.*,featured_game.featured_image.data.*,featured_game.featured_image_web.data.*,games.game_id.*,games.game_id.image.data.*,games.game_id.featured_image.data.*,games.game_id.access_codes.*,games.game_id.ages.age_range_id.*,games.game_id.gender.gender_id.*,games.game_id.interests.interests_id.*,games.game_id.so.so_id.*,games.game_id.states.state_id.*,games.game_id.access_codes.codes_id.image.data.*,games.game_id.promo.*");
            setAuthorizationToken(Constants.TOKEN_CMS);
            return await GetAsync<GameResponse>(uri);
        }
    }
}
