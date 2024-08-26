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

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosPaisController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;

        public EstadosPaisController(IMapper mapper, Db_DestappContext context)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EstadosPai>>>> GetEstadosPais()
        {
            var response = new ApiResponse<List<EstadosPai>>();
            try
            {
                var states = await (from ep in _context.EstadosPais select ep).ToListAsync();

                if (states.Count <= 0)
                    states = new List<EstadosPai>
                    {
                        new EstadosPai
                        {
                            Id = 0,
                            IdPais = null,
                            CitiesStates = null,
                            ClaveEstado = null,
                            NombreEstado = null
                        }
                    };
                response.Success = true;
                response.Result = mapper.Map<List<EstadosPai>>(states);
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

        // GET: api/GetEstadoById/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EstadosPai>>> GetEstadoById(int id)
        {
            var response = new ApiResponse<EstadosPai>();
            try
            {
                var states = await (from ep in _context.EstadosPais where ep.Id == id select ep).FirstOrDefaultAsync();
                var cities = await (from ce in _context.CitiesStates where ce.IdState == id select ce).ToListAsync();

                if (cities.Count  <= 0)
                    cities = new List<CitiesState> { new CitiesState { Id = 0, IdState = 0, IdStateNavigation = null, KeyCity = null, NameCity = null } };

                if (states == null)
                    states = new EstadosPai { Id = 0, IdPais = null, ClaveEstado = null, NombreEstado = null, CitiesStates = null };

                states.CitiesStates = cities;

                response.Success = true;
                response.Result = mapper.Map<EstadosPai>(states);
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