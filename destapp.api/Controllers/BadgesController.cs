using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public BadgesController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<ApiResponse<BadgesResponse>>> GetBatges(string idUser)
        {
            var response = new ApiResponse<BadgesResponse>();
            try
            {
                var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                DataBadges dataBadges = new DataBadges();
                var allBadges = await dataBadges.GetAllBadges();
                var badgesUser = await (from du in context.DatosUsuarios
                                  join ba in context.LogrosUsuarios on du.IdDatoUsuario equals ba.IdDatoUsuario
                                  where du.IdDatoUsuario.Equals(idUser)
                                  select ba).ToListAsync();

                if (badgesUser != null)
                    if (badgesUser.Count > 0)
                        allBadges.data.ForEach(x =>
                        {
                            badgesUser.ForEach(y =>
                            {
                                if (x.id == y.IdLogro)
                                {
                                    x.isGetting = true;
                                    x.isSelected = y.IsSelected;
                                }
                            });
                        });
                response.Result = mapper.Map<BadgesResponse>(allBadges);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }
}