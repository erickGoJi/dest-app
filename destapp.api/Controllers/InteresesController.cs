using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Mvc;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteresesController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public InteresesController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<ApiResponse<InteresesResponse>>> GetIntereses(string idUser)
        {
            var response = new ApiResponse<InteresesResponse>();
            try
            {
                var existUser = context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefault();
                if(existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                
                DataIntereses dataIntereses = new DataIntereses();
                var allIntereses = await dataIntereses.GetAllIntereses();
                var interesesUser = (from du in context.DatosUsuarios
                                join i in context.GustosUsuarios on du.IdDatoUsuario equals i.IdDatoUsuario
                                where du.IdDatoUsuario.Equals(idUser)
                                select new GustosUsuario
                                {
                                    IdGusto = Convert.ToInt32(i.IdGusto)
                                }).ToList();

                if (interesesUser != null)
                    allIntereses.data.ForEach(x =>
                    {
                        interesesUser.ForEach(y =>
                        {
                            if (x.id == y.IdGusto)
                                x.isSelected = true;
                        });
                    });
                    response.Result = mapper.Map<InteresesResponse>(allIntereses);
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
