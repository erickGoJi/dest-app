using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Request;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models.Torneo;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TorneoController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public TorneoController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public ActionResult<ApiResponse<TorneoResponse>> GetAllNoUser()
        {
            var response = new ApiResponse<TorneoResponse>();
            //try
            //{
            //    apiClient.CoreApiClient.Torneo game = new apiClient.CoreApiClient.Torneo();
            //    TorneoResponse torneoresponse = await game.getAllTorneos();
            //    foreach(apiClient.Models.Torneo.Torneo torneo in torneoresponse.data.ToList())
            //    {
            //        var date = DateTime.Now;
            //        if (date.CompareTo(torneo.end_date) > 0)
            //        {
            //            torneoresponse.data.Remove(torneo);
            //            continue;
            //        }
            //    }
            //    response.Result = mapper.Map<TorneoResponse>(torneoresponse);
            //}
            //catch (Exception ex)
            //{
            //    response.Result = null;
            //    response.Success = false;
            //    response.Message = ex.Message;
            //    return StatusCode(500, response);
            // }
            return Ok(response);
        }

        [HttpPost("startmatch")]
        [AllowAnonymous]
        public ActionResult<ApiResponse<IniciarTorneoResponse>> StartMatchCoins(IniciarTorneoRequest torneoRequest)
        {
            var response = new ApiResponse<IniciarTorneoResponse>();
            //try
            //{
            //    var user = this.context.DatosUsuarios.Find(torneoRequest.idUsuario);

            //    if (user == null)
            //    {
            //        response.Success = false;
            //        response.Message = $"Usuario id '{ torneoRequest.idUsuario }' Not Found";
            //        return NotFound(response);
            //    }

            //    var team = (from a in context.TorneoTeams
            //                    join b in context.TorneoTeamUsers on a.Id equals b.IdTorneoTeam
            //                    where a.IdTorneo == torneoRequest.idTorneo && b.IdUser == torneoRequest.idUsuario
            //                    select new Team
            //                    {
            //                        torneoTeam = a,
            //                        torneoTeamUser = b
            //                    }).FirstOrDefault();
            //    if (team == null)
            //    {
            //        team = new Team();
            //        if (torneoRequest.torneo_type == 1)
            //        {
            //            team.torneoTeam = new TorneoTeam
            //            {
            //                IdTorneo = torneoRequest.idTorneo,
            //                Type = 1,
            //                CreatedAt = DateTime.Now
            //            };
            //            context.Add(team.torneoTeam);
            //            context.SaveChanges();

            //            team.torneoTeamUser = new TorneoTeamUser
            //            {
            //                IdTorneoTeam = team.torneoTeam.Id,
            //                IdUser = torneoRequest.idUsuario,
            //                Status = true,
            //                CreatedAt = DateTime.Now,
            //                UpdatedAt = DateTime.Now,
            //            };
            //            context.Add(team.torneoTeamUser);
            //            context.SaveChanges();
            //        }
            //        else
            //        {
            //            response.Success = false;
            //            response.Message = $"No puedes jugar ya que no estas asignado a ningún equipo";
            //            return NotFound(response);
            //        }
            //    }
            //    var partida = new JuegosUsuariosPartida
            //    {
            //        IdUsuario = torneoRequest.idUsuario,
            //        IdJuego = torneoRequest.idJuego,
            //        IdTorneoTeam = team.torneoTeam.Id,
            //        Score = 0,
            //        CreatedAt = DateTime.Now,
            //        UpdatedAt = DateTime.Now

            //    };
            //    context.Add(partida);
            //    context.SaveChanges();

            //    if (torneoRequest.access_type == 4) // coins
            //    {

            //        ManageCoins manageCoins = new ManageCoins(context);
            //        var userCoinsResponse = manageCoins.manageCoins(torneoRequest.idUsuario, (-1 * torneoRequest.cantCoins), 1, partida.Id); //tipo 2 es trivias
            //        if (userCoinsResponse == null)
            //        {
            //            //context.Remove(partida);
            //            context.SaveChanges();
            //            response.Success = false;
            //            response.Message = $"El usuario '{user.NombreUsuario }' no cuenta con suficientes coins";
            //            return Conflict(response);
            //        }
            //    }

            //    if (torneoRequest.access_type == 2) // barcode
            //    {

            //    }

            //    if (torneoRequest.access_type == 3) // imagen
            //    {

            //    }

            //    if (torneoRequest.access_type == 1) // free
            //    {

            //    }


            //    response.Success = true;
            //    response.Message = "Partida iniciada con éxito";
            //    DataAvatar dataAvatar = new DataAvatar();
            //    var resultAvatar = await dataAvatar.GetAllAvatar();
            //    var avatares = resultAvatar.data;
            //    response.Result = new IniciarTorneoResponse()
            //    {
            //        idPartida = partida.Id,
            //        idUsuario = torneoRequest.idUsuario,
            //        userName = user.NombreUsuario,
            //        avatar = (avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault() != null) ?
            //                            avatares.Where(x1 => x1.id == user.IdAvatar).FirstOrDefault().image.data.full_url :
            //                            null
            //    };
            //}catch(Exception ex)
            //{
            //    response.Success = false;
            //    response.Message = ex.InnerException.Message;
            //    return Conflict(response);
            //}
            return response;
        }

        public class Team
        {
            public TorneoTeam torneoTeam { get; set; }
            public TorneoTeamUser torneoTeamUser { get; set; }
        }

    }
}