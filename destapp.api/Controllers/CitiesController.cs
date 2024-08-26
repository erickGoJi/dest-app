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
    public class CitiesController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public CitiesController(IMapper mapper, Db_DestappContext _contex)
        {
            this._context = _contex;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CitiesState>>>>GetAll(){
            var response = new ApiResponse<List<CitiesState>>();
            try
            {
                var result = (from ce in _context.CitiesStates select ce).ToList();
                response.Success = true;
                response.Result = mapper.Map<List<CitiesState>>(result);
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