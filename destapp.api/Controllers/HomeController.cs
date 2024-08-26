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
using static destapp.apiClient.Models.HomeResponse;
using Microsoft.AspNetCore.Authorization;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public HomeController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }
       

        [HttpGet("{idUser}")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApiResponse<HomeResponse>>>GetHomeUser(string idUser)
        {
            var response = new ApiResponse<HomeResponse>();
            try
            {
                var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                DataHome dataHome = new DataHome();
                var allHomes = await dataHome.GetAllHome();

                foreach (Prize pz in allHomes.data[0].prizes.ToList())
                {
                    var date = DateTime.Now;
                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize.start_date)) > 0)
                    {
                        pz.prize.habilitado = true;
                    }
                    else
                    {
                        pz.prize.habilitado = false;
                    }
                    // quitar si la fecha final ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize.finish_date)) > 0)
                    {
                        allHomes.data[0].prizes.Remove(pz);
                    }
                }

                foreach (Prize pz in allHomes.data[0].prizes_two.ToList())
                {
                    var date = DateTime.Now;
                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize.start_date)) > 0)
                    {
                        pz.prize.habilitado = true;
                    }
                    else
                    {
                        pz.prize.habilitado = false;
                    }
                    // quitar si la fecha final ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize.finish_date)) > 0)
                    {
                        allHomes.data[0].prizes_two.Remove(pz);
                    }
                }

                // set interest_reward 
                var interest_rewards = context.InterestRewards.Where(f => f.IdUser == idUser);
                foreach (InterestReward ir in interest_rewards)
                {
                    foreach (Prize pz in allHomes.data[0].prizes)
                    {
                        if (ir.IdReward == pz.prize.id)
                        {
                            pz.prize.me_interesa = true;
                        }
                    }

                    foreach (Prize pz in allHomes.data[0].prizes_two)
                    {                           
                        if (ir.IdReward == pz.prize.id)
                        {
                            pz.prize.me_interesa = true;
                        }
                    }
                }

                foreach (FeaturedContent pz in allHomes.data[0].featured_content.ToList())
                {
                    var date = DateTime.Now;
                    if (date.CompareTo(DateTime.Parse(pz.featured_content_id.end_date)) > 0)
                    {
                        allHomes.data[0].featured_content.Remove(pz);
                        continue;
                    }
                }

                // set avalibre days
                var pz_especiales = allHomes.data[0].prizes;
                var pz_especiales_two = allHomes.data[0].prizes_two;

                //arma arreglo con recompensas canjeadas que no son especiales
                var ex_events = context.ExchangeProductHistories.Where(f => f.IdUser == idUser).ToList();
                foreach (ExchangeProductHistory eph in ex_events.ToList())
                {
                    foreach (Prize pze in pz_especiales)
                    {
                        if (eph.IdProduct == pze.prize.id && pze.prize.category.id == 7)
                            ex_events.Remove(eph);
                    }
                    foreach (Prize pze_two in pz_especiales_two)
                    {
                        if (eph.IdProduct == pze_two.prize.id && pze_two.prize.category.id == 7)
                            ex_events.Remove(eph);
                    }
                }

                DateTime date_last_ex = new DateTime(); DateTime date_avalible = new DateTime();

                
                if (ex_events.Count() > 0) //en caso de que si existan canjes que no son especiales
                {
                    var latest_exch_event = ex_events.OrderBy(f => f.CreatedAt).Last();
                    date_last_ex = new DateTime(latest_exch_event.CreatedAt.Value.Year, latest_exch_event.CreatedAt.Value.Month, latest_exch_event.CreatedAt.Value.Day);
                    date_avalible = date_last_ex.AddMonths(1);
                    DateTime today = DateTime.Now;
                    today = new DateTime(today.Year, today.Month, today.Day);
                    TimeSpan ts =   date_avalible - today;
                    int differenceInDays = ts.Days;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (Prize pz in allHomes.data[0].prizes)
                    {

                        if (pz.prize.category.id != 7)
                        {
                            pz.prize.dias_habilitado = differenceInDays;
                        }
                    }

                    foreach (Prize pz_two in allHomes.data[0].prizes_two)
                    {

                        if (pz_two.prize.category.id != 7)
                        {
                            pz_two.prize.dias_habilitado = differenceInDays;
                        }
                    }

                    allHomes.data[0].dias_habilitado = differenceInDays;
                    allHomes.data[0].fecha_canjeo = dateString;
                        
                }
                else
                {
                    allHomes.data[0].dias_habilitado = 0;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");
                    allHomes.data[0].fecha_canjeo = dateString;
                }
                response.Result = mapper.Map<HomeResponse>(allHomes);
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

        [HttpGet("GetHomes")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> GetHomes()
        {
            var response = new ApiResponse<HomeResponse>();
            try
            {
                
                DataHome dataHome = new DataHome();
                var allHomes = await dataHome.GetAllHome();
                response.Result = mapper.Map<HomeResponse>(allHomes);

                foreach (Prize pz in allHomes.data[0].prizes.ToList())
                {
                    var date = DateTime.Now;
                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize.start_date)) > 0)
                    {
                        pz.prize.habilitado = true;
                    }
                    else
                    {
                        pz.prize.habilitado = false;
                    }
                    // quitar si la fecha final ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize.finish_date)) > 0)
                    {
                        allHomes.data[0].prizes.Remove(pz);
                    }
                }

                foreach (Prize pz in allHomes.data[0].prizes_two.ToList())
                {
                    var date = DateTime.Now;
                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize.start_date)) > 0)
                    {
                        pz.prize.habilitado = true;
                    }
                    else
                    {
                        pz.prize.habilitado = false;
                    }
                    // quitar si la fecha final ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize.finish_date)) > 0)
                    {
                        allHomes.data[0].prizes_two.Remove(pz);
                    }
                }
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