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

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderDbController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public GenderDbController(IMapper mapper, Db_DestappContext _context)
        {
            this._context = _context;
            this.mapper = mapper;
        }

        //Obtiene todos los Géneros del cms
        // GET: api/Genero
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<GenderDb>>>> GetAll()
        {
            var response = new ApiResponse<List<GenderDb>>();
            try
            {
                var result = (from genero in _context.GenderDbs select genero).ToList();
                response.Success = true;
                response.Result = mapper.Map<List<GenderDb>>(result);

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