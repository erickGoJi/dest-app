using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Request;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static destapp.api.Models.Response.RankingResponse;
using static destapp.apiClient.Models.GameResponse;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JuegoController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public JuegoController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{idUser}/all")]
        public async Task<ActionResult<ApiResponse<GameResponse>>> GetAll(string idUser)
        {
            var response = new ApiResponse<GameResponse>();
            try
            {                
                apiClient.CoreApiClient.Game game = new apiClient.CoreApiClient.Game();
                GameResponse gamesresponse = await game.getAllGames();
                foreach (DataGame data in gamesresponse.data)
                    foreach (GamePrivate gp in data.games.ToList())
                    {
                        var date = DateTime.Now;
                        if (date.CompareTo(gp.game_id.end_date) > 0)
                        {
                            data.games.Remove(gp);
                            continue;
                        }
                        if(gp.game_id.promo != null)
                            if(date.CompareTo(gp.game_id.promo.start_date) < 0 || date.CompareTo(gp.game_id.promo.end_date) > 0)
                                gp.game_id.promo = null;
                        try
                        {
                            Score biggerScore = (from a in context.JuegosUsuariosPartidas
                                                 join b in context.JuegosBigScoreUsuarioPartida on a.Id equals b.IdPartida
                                                 where a.IdJuego == gp.game_id.idCMS && a.IdUsuario == idUser
                                                 select new Score
                                                 {
                                                     idJuego = a.IdJuego,
                                                     idPartida = a.Id,
                                                     idUsuario = b.IdUsuario,
                                                     score = a.Score
                                                 })
                                 .OrderByDescending(d => d.score).FirstOrDefault();
                            gp.game_id.biggerScore = biggerScore;
                            gp.game_id.id = gp.game_id.idCMS;
                            if (data.featured_game.idCMS == gp.game_id.idCMS)
                            {
                                data.featured_game.biggerScore = biggerScore;
                                data.featured_game.id = gp.game_id.idCMS;
                            }
                        }catch(Exception e)
                        {

                        }   
                        //gameModel.idCMS
                    }
                        
                response.Result = mapper.Map<GameResponse>(gamesresponse);
            }catch(Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost("endMatch/{endGameMatchStringRequest}")]
        [AllowAnonymous]
        public ActionResult<ApiResponse<EndGameMatchResponse>> endMatch(string endGameMatchStringRequest)
        {
           byte[] endGameMatachDecodeBytes = Convert.FromBase64String(endGameMatchStringRequest);
            string decodeendGameMatch = Encoding.UTF8.GetString(endGameMatachDecodeBytes);
            EndGameMatchRequest endGameMatchRequest = JsonConvert.DeserializeObject<EndGameMatchRequest>(decodeendGameMatch);
            var response = new ApiResponse<EndGameMatchResponse>();
            var user = this.context.DatosUsuarios.Find(endGameMatchRequest.idUsuario);
            if(user == null)
            {
                response.Success = false;
                response.Message = $"Usuario id '{ endGameMatchRequest.idUsuario }' Not Found";
                return NotFound(response);
            }
            var partida = this.context.JuegosUsuariosPartidas.Find(endGameMatchRequest.idPartida);
            if(partida == null)
            {
                response.Success = false;
                response.Message = $"Partida '{ endGameMatchRequest.idPartida }' No Encontrada";
                return NotFound(response);
            }

            partida.Score = endGameMatchRequest.score * 10;
            context.Entry(partida).State = EntityState.Modified;
            var biggerScore = this.context.JuegosBigScoreUsuarioPartida.Where(x => x.IdUsuario.Equals(endGameMatchRequest.idUsuario) && x.IdPartidaNavigation.IdJuego == partida.IdJuego).FirstOrDefault();
            if(biggerScore == null)
            {
                biggerScore = new JuegosBigScoreUsuarioPartidum()
                {
                    IdPartida = endGameMatchRequest.idPartida,
                    IdUsuario = endGameMatchRequest.idUsuario,
                    Score = partida.Score,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Entry(biggerScore).State = EntityState.Added;
            }
            else
            {
                biggerScore.UpdatedAt = DateTime.Now;
                context.Entry(biggerScore).State = EntityState.Modified;
            }
            if(biggerScore.Score <= partida.Score)
            {
                biggerScore.Score = partida.Score;
                biggerScore.IdPartida = endGameMatchRequest.idPartida;
            }
            context.SaveChanges();
            var tickets = endGameMatchRequest.score;
           // if(endGameMatchRequest.promo != null && endGameMatchRequest.promo > 0)
            if(endGameMatchRequest.promo != null)
            {
                tickets *= (int)endGameMatchRequest.promo;
            }

            if (partida.IdJuego == 9)
            {
                tickets = 0;
            }

            ManageTickets manageTickets = new ManageTickets(context);
            var ticketsUserResponse = manageTickets.manageTickets(endGameMatchRequest.idUsuario, tickets, 1, endGameMatchRequest.idPartida);
            TicketsUserHistory ticketsUserHistory;
            if (ticketsUserResponse != null)
            {
                ticketsUserHistory = ticketsUserResponse.userTicketsLog;
            }
            else
            {
                return null;
            }
            response.Success = true;
            response.Message = "Score actualizado con éxito!";
            response.Result = new EndGameMatchResponse()
            {
                idBiggerScore = biggerScore.Id,
                biggerScore = biggerScore.Score,
                idJuego = partida.IdJuego,
                ticketsWon = ticketsUserHistory.Tickets,
                idPartida = partida.Id,
                idUsuario = endGameMatchRequest.idUsuario,
                score = endGameMatchRequest.score
            };
            return response;
        }

        [HttpPost("startMatch")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<StartMatchResponse>>> startNewMatchAsync(GameMatchRequest gameMatchRequest)
        {
            var response = new ApiResponse<StartMatchResponse>();
            try
            {
                var user = this.context.DatosUsuarios.Find(gameMatchRequest.idUsuario);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ gameMatchRequest.idUsuario }' Not Found";
                    return NotFound(response);
                }

                var partida = new JuegosUsuariosPartida()
                {
                    IdUsuario = gameMatchRequest.idUsuario,
                    IdJuego = gameMatchRequest.idJuego,
                    Score = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Add(partida);
                context.SaveChanges();
                ManageCoins manageCoins = new ManageCoins(context);
                var userCoinsResponse = manageCoins.manageCoins(gameMatchRequest.idUsuario, gameMatchRequest.cantCoins, 1, partida.Id);
                if (userCoinsResponse == null)
                {
                    context.Remove(partida);
                    context.SaveChanges();
                    response.Success = false;
                    response.Message = $"El usuario '{user.NombreUsuario }' no cuenta con suficientes coins";
                    return Conflict(response);
                }
                var userCoins = userCoinsResponse.userCoins as CoinsUser;
                var userCoinsLog = userCoinsResponse.userCoinsLog as CoinsUserHistory;
                response.Success = true;
                response.Message = "Partida iniciada con éxito";
                DataAvatar dataAvatar = new DataAvatar();
                var resultAvatar = await dataAvatar.GetAllAvatar();
                var avatares = resultAvatar.data;
                response.Result = new StartMatchResponse()
                {
                    idPartida = partida.Id,
                    idUsuario = gameMatchRequest.idUsuario,
                    userName = user.NombreUsuario,
                    avatar = (avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault() != null) ?
                                        avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault().image.data.full_url :
                                        null,
                    idCoinsUser = userCoins.Id,
                    idCoinsUserHistory = userCoinsLog.Id,
                    coinsUser = userCoins.Coins
                };
            }catch(Exception ex)
            {

            }
            return response;
        }

        [HttpGet("getRanking/{idUser}/{idJuego}/{top}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<RankingResponse>>> GetRanking(string idUser,int idJuego, int top)
        {
           var response = new ApiResponse<RankingResponse>();
            try
            {
                RankingResponse rankingResponse = new RankingResponse();
                DataAvatar dataAvatar = new DataAvatar();
                var resultAvatar = await dataAvatar.GetAllAvatar();
                var avatares = resultAvatar.data;
                var rankingList = (from bigScore in context.JuegosBigScoreUsuarioPartida join
                partida in context.JuegosUsuariosPartidas on bigScore.IdPartida equals partida.Id
                             join user in context.DatosUsuarios on bigScore.IdUsuario equals user.IdDatoUsuario
                             orderby bigScore.Score descending
                             where partida.IdJuego == idJuego
                             select new RankingDatum
                             {
                                 idUser = bigScore.IdUsuario,
                                 fullUrlAvatar = (avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault() != null) ?
                                    avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault().image.data.full_url :
                                    null,
                                 nameUser = user.NombreUsuario,
                                 userScore = bigScore.Score,
                                 idAvatar = user.IdAvatar,
                                 position = (from bigScore2 in context.JuegosBigScoreUsuarioPartida
                                        join partida2 in context.JuegosUsuariosPartidas on bigScore2.IdPartida equals partida2.Id
                                        where partida2.IdJuego == idJuego where bigScore2.Score > bigScore.Score select bigScore2).Count()+1
                             }).Take(top).ToList();
                var rankingUser = (from bigScore in context.JuegosBigScoreUsuarioPartida
                                   join partida in context.JuegosUsuariosPartidas on bigScore.IdPartida equals partida.Id
                                   join user in context.DatosUsuarios on bigScore.IdUsuario equals user.IdDatoUsuario
                                   orderby bigScore.Score descending
                                   where partida.IdJuego == idJuego
                                   where bigScore.IdUsuario == idUser
                                   select new RankingDatum
                                   {
                                       idUser = bigScore.IdUsuario,
                                       fullUrlAvatar = (avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault() != null) ?
                                          avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault().image.data.full_url :
                                          null,
                                       nameUser = user.NombreUsuario,
                                       userScore = bigScore.Score,
                                       idAvatar = user.IdAvatar,
                                       position = (from bigScore2 in context.JuegosBigScoreUsuarioPartida
                                                   join partida2 in context.JuegosUsuariosPartidas on bigScore2.IdPartida equals partida2.Id
                                                   where partida2.Id == idJuego
                                                   where bigScore2.Score > bigScore.Score
                                                   select bigScore2).Count() + 1
                                   }).FirstOrDefault();
                rankingResponse.listData = rankingList;
                rankingResponse.dataData = rankingUser;
                response.Result = rankingResponse;
            }catch(Exception e)
            {

            }
            /*try
            {
                var urlAvatar = "";
                var positionUser = 0;
                DataAvatar dataAvatar = new DataAvatar();
                var resultAvatar = await dataAvatar.GetAllAvatar();

                var rankingAll = await (from r in context.JuegosBigScoreUsuarioPartidas orderby r.Score descending select r).ToListAsync();
                var ranking = await (from r in context.JuegosBigScoreUsuarioPartidas orderby r.Score descending select r).Take(top).ToListAsync();
                var score = await (from s in context.JuegosBigScoreUsuarioPartidas where s.IdUsuario.Equals(idUser) select s).FirstOrDefaultAsync();
                var user = await (from d in context.DatosUsuarios where d.IdDatoUsuario.Equals(idUser) select d).FirstOrDefaultAsync();

                RankingResponse rankingResponse = new RankingResponse();
                if (score == null)
                    score = new JuegosBigScoreUsuarioPartida();
                rankingAll.ForEach(a =>
                {
                    if (a.IdUsuario.Equals(user.IdDatoUsuario))
                        positionUser = rankingAll.IndexOf(a) + 1;
                });

                rankingResponse.dataData.idUser = user.IdDatoUsuario;
                rankingResponse.dataData.idAvatar = user.IdAvatar;
                rankingResponse.dataData.fullUrlAvatar = "";
                rankingResponse.dataData.nameUser = user.NombreUsuario;
                rankingResponse.dataData.userScore = score.Score;
                rankingResponse.dataData.position = positionUser;

                ranking.ForEach(x =>
                {
                    positionUser = ranking.IndexOf(x) + 1;
                    var userA = (from d in context.DatosUsuarios where d.IdDatoUsuario.Equals(x.IdUsuario) select d).FirstOrDefaultAsync().GetAwaiter().GetResult();
                    var scoreA = (from s in context.JuegosBigScoreUsuarioPartidas where s.IdUsuario.Equals(x.IdUsuario) select s).FirstOrDefaultAsync().GetAwaiter().GetResult();
                    if (scoreA == null)
                        scoreA = new JuegosBigScoreUsuarioPartida();
                    resultAvatar.data.ForEach(y =>
                    {
                        if (y.id == userA.IdAvatar)
                            urlAvatar = y.image.data.full_url;
                    });
                    rankingResponse.listData.Add(new RankingResponse.RankingDatum { idUser = x.IdUsuario, idAvatar = userA.IdAvatar, fullUrlAvatar = urlAvatar, nameUser = userA.NombreUsuario, userScore = scoreA.Score, position = positionUser });
                });

                response.Success = true;
                response.Result = mapper.Map<RankingResponse>(rankingResponse);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.Message);
                return StatusCode(500, response);
            }*/
            return Ok(response);
        }

        [HttpGet("GetAllNoUser")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<GameResponse>>> GetAllNoUser()
        {
            var response = new ApiResponse<GameResponse>();
            try
            {
                apiClient.CoreApiClient.Game game = new apiClient.CoreApiClient.Game();
                GameResponse gamesresponse = await game.getAllGames();
                response.Result = mapper.Map<GameResponse>(gamesresponse);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                return StatusCode(500, response);
            }
            return Ok(response);
        }



        /*public dynamic manageCoins(string idUsuario,int cantCoins,int type, int source)
        {
            var userCoins = this.context.CoinsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if(userCoins == null)
            {
                userCoins = new CoinsUser();
                userCoins.IdUsuario = idUsuario;
                userCoins.CreatedAt = DateTime.Now;
                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Added;
            }
            else
            {

                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Modified;
            }
            if(cantCoins < 0)
            {
                if (userCoins.Coins < (cantCoins * -1))
                {
                    if (context.Entry(userCoins).State == (EntityState.Added))
                        context.SaveChanges();
                    //response.Success = false;
                    return null;
                }
            }
            var userCoinsLog = new CoinsUserHistory()
            {
                Coins = cantCoins,
                IdType = type,
                IdUsuario = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            context.Add(userCoinsLog);
            userCoins.Coins += cantCoins;
            context.SaveChanges();
            
            return new { userCoins, userCoinsLog };
        }*/

        /*public dynamic manageTickets(string idUsuario, int cantTickets, int type, int source)
        {
            var userTickets = this.context.TicketsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if (userTickets == null)
            {
                userTickets = new TicketsUser();
                userTickets.IdUsuario = idUsuario;
                userTickets.CreatedAt = DateTime.Now;
                userTickets.UpdatedAt = DateTime.Now;
                context.Entry(userTickets).State = EntityState.Added;
            }
            else
            {

                userTickets.UpdatedAt = DateTime.Now;
                context.Entry(userTickets).State = EntityState.Modified;
            }
            if (cantTickets < 0)
            {
                if (userTickets.Tickets < (cantTickets * -1))
                {
                    if (context.Entry(userTickets).State == (EntityState.Added))
                        context.SaveChanges();
                    //response.Success = false;
                    return null;
                }
            }
            var userTicketsLog = new TicketsUserHistory()
            {
                Tickets = cantTickets,
                IdType = type,
                IdUser = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            context.Add(userTicketsLog);
            userTickets.Tickets += cantTickets;
            context.SaveChanges();

            return new { userTickets, userTicketsLog };
        }*/

    }
}