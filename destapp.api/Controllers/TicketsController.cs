using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
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
    public class TicketsController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public TicketsController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<TicketsUserHistory>>>> GetAll()
        {
            var response = new ApiResponse<List<TicketsUserHistory>>();
            try
            {
                var allTickets = (from t in _context.TicketsUsers select t).ToListAsync();
                response.Success = true;
                response.Result = mapper.Map<List<TicketsUserHistory>>(allTickets);
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

        /*// GET: api/DatosUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<List<TicketsUserHistory>>>> GetTicketsByIdUser(string id)
        {
            var response = new ApiResponse<List<TicketsUserHistory>>();
            try
            {
                var ticketsUser = (from tu in _context.TicketsUserHistories where tu.IdUser.Equals(id) select tu).ToList();

                response.Success = true;
                response.Result = mapper.Map<List<TicketsUserHistory>>(ticketsUser);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }*/

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TicketsUserHistory>>> PostTicketsUser(TicketsUserHistory ticketsUserHistory)
        {
            var response = new ApiResponse<TicketsUserHistory>();
            //await _context.TicketsUserHistories.AddAsync(ticketsUserHistory);
            //await _context.TicketsUsers.AddAsync(new TicketsUser
            //{
            //    IdUsuario = ticketsUserHistory.IdUser,
            //    Tickets = ticketsUserHistory.Tickets,
            //    CreatedAt = DateTime.Now,
            //    UpdatedAt = DateTime.Now
            //});
            ManageTickets manageTickest = new ManageTickets(_context);
            var userTicketsResponse = manageTickest.manageTickets(ticketsUserHistory.IdUser, ticketsUserHistory.Tickets, ticketsUserHistory.IdType, ticketsUserHistory.Source); //tipo 2 es trivias

            try
            {
                _context.SaveChanges();
                response.Success = true;
                response.Result = mapper.Map<TicketsUserHistory>(ticketsUserHistory);
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