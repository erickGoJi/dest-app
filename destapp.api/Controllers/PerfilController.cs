using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.apiClient.CoreApiClient;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;

        public PerfilController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<ApiResponse<PerfilResponse>>> GetDataPerfil(string idUser)
        {
            var response = new ApiResponse<PerfilResponse>();
            try
            {
                var user = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(idUser) select u).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }

                var interestUser = await (from i in _context.GustosUsuarios where i.IdDatoUsuario.Equals(idUser) select i).ToListAsync();
                var badgesUser = await (from b in _context.LogrosUsuarios where b.IdDatoUsuario.Equals(idUser) select b).ToListAsync();

                var perfil = new PerfilResponse();
                DataAvatar dataAvatar = new DataAvatar();
                DataIntereses dataIntereses = new DataIntereses();
                DataBadges dataBadges = new DataBadges();
                var allAvatar = await dataAvatar.GetAllAvatar();
                var allInterest = await dataIntereses.GetAllIntereses();
                var allBadges = await dataBadges.GetAllBadges();

                perfil.data.gender.data = await (from g in _context.GenderDbs select g).ToListAsync();
                perfil.data.states.data = await (from s in _context.EstadosPais select s).ToListAsync();
                if (user.IdEstado != null)
                    perfil.data.cities.data = await (from c in _context.CitiesStates where c.IdState == user.IdEstado select c).ToListAsync();
                perfil.data.avatar = ManageCatalogues.AvatarSelectedByIdUser(allAvatar, user.IdAvatar);
                perfil.data.interest = ManageCatalogues.InterestSelectedByIdUser(allInterest, interestUser);
                perfil.data.badges = ManageCatalogues.BadgeSelectedByIdUser(allBadges, badgesUser);

                response.Success = true;
                response.Result = mapper.Map<PerfilResponse>(perfil);
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