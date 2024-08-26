using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using destapp.apiClient.Models;
using destapp.apiClient.CoreApiClient;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public AvatarController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Obtiene todos los avatares del cms
        // GET: api/Avatar
        [HttpGet]
        public async Task<ActionResult<ApiResponse<AvatarResponse>>> GetAll()
        {
            var response = new ApiResponse<AvatarResponse>();
            try
            {
                DataAvatar dataAvatar = new DataAvatar();
                var result = await dataAvatar.GetAllAvatar();
                response.Success = true;
                response.Result = mapper.Map<AvatarResponse>(result);

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

        [HttpGet("{idUser}")]
        public async Task<ActionResult<ApiResponse<AvatarResponse>>>GetAvatar(string idUser)
        {
            var response = new ApiResponse<AvatarResponse>();
            try
            {
                var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                DataAvatar dataAvatar = new DataAvatar();
                    var allAvatars = await dataAvatar.GetAllAvatar();
                    var avataresUser = await (from du in context.DatosUsuarios
                                        where du.IdDatoUsuario.Equals(idUser)
                                        select du).FirstOrDefaultAsync();

                    if(avataresUser != null)
                    if(avataresUser.IdAvatar != null)
                    allAvatars.data.ForEach(x =>
                    {
                        if (x.id == avataresUser.IdAvatar)
                            x.isSelected = true;
                    });
                    response.Result = mapper.Map<AvatarResponse>(allAvatars);
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