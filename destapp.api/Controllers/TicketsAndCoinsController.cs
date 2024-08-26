using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsAndCoinsController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;

        public TicketsAndCoinsController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<ApiResponse<TicketsAndCoinsAndAvatarResponse>>> GetTicketsAndCoinsAndAvatarByIdUser(string idUser)
        {
            var response = new ApiResponse<TicketsAndCoinsAndAvatarResponse>();
            try
            {
                var existUser = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(idUser) select u).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }

                var tickets = await (from t in _context.TicketsUsers where t.IdUsuario.Equals(idUser) select t).FirstOrDefaultAsync();
                if(tickets == null)
                    tickets = new TicketsUser();

                var coins = await (from c in _context.CoinsUsers where c.IdUsuario.Equals(idUser) select c).FirstOrDefaultAsync();
                if(coins == null)
                    coins = new CoinsUser();

               // var maxScore = await (from s in _context.JuegosUsuariosPartidas where s.IdUsuario.Equals(idUser) orderby s.Score descending select s).FirstOrDefaultAsync();
               // if(maxScore == null)
                //    maxScore = new JuegosUsuariosPartida();

                ManageAvatar manageAvatar = new ManageAvatar(_context);
                var avatar = await manageAvatar.GetAvatarByIdUser(idUser);
                if(avatar == null)
                    avatar = new apiClient.Models.AvatarResponse.Datum();

                long notRead = await (from n in _context.Notifications where n.IdUser.Equals(idUser) && n.DateRead == null select n).CountAsync();


                var data = new TicketsAndCoinsAndAvatarResponse();
                data.tickets = tickets;
                data.coins = coins;
                data.avatar = avatar;
                data.score = null;
                data.user = existUser;
                data.notificationNotRead = notRead;

                response.Success = true;
                response.Result = mapper.Map<TicketsAndCoinsAndAvatarResponse>(data);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Result = null;
                response.Message = string.Format("Internal server error: {0}", ex.Message);
                return StatusCode(500, response);
            }
                return Ok(response);
        }
    }
}