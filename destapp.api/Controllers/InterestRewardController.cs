using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterestRewardController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public InterestRewardController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpPost("{isWeb?}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<InterestReward>>> PostInterestReward(InterestReward interest, bool isWeb = false)    
        {
            if (isWeb)
            {
                Console.WriteLine("is web");
            }
            else
            {
                Console.WriteLine("is not web");
            }
            var response = new ApiResponse<InterestReward>();
            var user = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(interest.IdUser) select u).FirstOrDefaultAsync();
            if(user == null)
            {
                response.Success = false;
                response.Message = $"User id { interest.IdUser } Not Found";
                return NotFound(response);
            }
            interest.CreatedAt = DateTime.Now;
            await _context.InterestRewards.AddAsync(interest);
            try
            {
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Result = mapper.Map<InterestReward>(interest);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}.", ex.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

      
        [HttpPost("PostInterestTrivia/{isWeb?}")]
        public async Task<ActionResult<ApiResponse<InteresesTrivia>>> PostInterestTrivia(int id_trivia, string id_user, bool isWeb = false)
        {
            if (isWeb)
            {
                Console.WriteLine("is web");
            }
            else
            {
                Console.WriteLine("is not web");
            }
            var response = new ApiResponse<InteresesTrivia>();
            var user = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(id_user) select u).FirstOrDefaultAsync();
            if (user == null)
            {
                response.Success = false;
                response.Message = $"User id { id_user } Not Found";
                return NotFound(response);
            }
            InteresesTrivia interest = new InteresesTrivia();
            interest.IdUser = id_user;
            interest.IdTrivia = id_trivia;
            interest.PublicationDate = DateTime.Now;
            interest.CreatedAt = DateTime.Now;

            await _context.InteresesTrivias.AddAsync(interest);
            try
            {
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Result = mapper.Map<InteresesTrivia>(interest);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}.", ex.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        
    }
}