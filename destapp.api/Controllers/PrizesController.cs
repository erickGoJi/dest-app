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
using static destapp.apiClient.Models.PrizeResponse;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrizesController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public PrizesController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{idUser}")]
       // [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PrizeResponse>>> GetHomeUser(string idUser)
        {
            var response = new ApiResponse<PrizeResponse>();
            try
            {
                var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                DataPrizes dataPrizes = new DataPrizes();
                var allPrizes = await dataPrizes.GetAllPrizes();

                //validación de fechas
                foreach (Prize_pz pz in allPrizes.data[0].prizes.ToList())
                {
                    var date = DateTime.Now;
                    //quitar cuando al fecha ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize_id.finish_date)) > 0)
                    {
                        allPrizes.data[0].prizes.Remove(pz);
                    }

                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize_id.start_date)) > 0)
                    {
                        pz.prize_id.habilitado = true;
                    }
                    else
                    {
                        pz.prize_id.habilitado = false;
                    }
                }

                //validación de stock 
                foreach (Prize_pz pz in allPrizes.data[0].prizes.ToList())
                {
                    var date = DateTime.Now;
                    //quitar cuando al fecha ya caduco
                    if ((pz.prize_id.stock - pz.prize_id.stock_used) < 1)
                    {
                        allPrizes.data[0].prizes.Remove(pz);
                    }
                }

                // set interest_reward 
                var interest_rewards = context.InterestRewards.Where(f => f.IdUser == idUser);
                foreach (InterestReward ir in interest_rewards)
                {
                    foreach (Prize_pz pz in allPrizes.data[0].prizes)
                    {
                        if (ir.IdReward == pz.prize_id.id)
                        {
                            pz.prize_id.me_interesa = true;
                        }
                    }

                }

                // set interest_reward featured 

                var result = await interest_rewards.FirstOrDefaultAsync(x => x.IdReward == allPrizes.data[0].featured_prize[0].prize_id.id);
                if (result != null)
                    allPrizes.data[0].featured_prize[0].prize_id.me_interesa = true;

                // set avalibre days
                var pz_especiales = allPrizes.data[0].prizes.Where(pz => pz.prize_id.category.id == 7);
                var ex_events = context.ExchangeProductHistories.Where(f => f.IdUser == idUser ).ToList();
                 foreach (ExchangeProductHistory eph in ex_events.ToList())
                {
                    foreach (Prize_pz pze in pz_especiales)
                    {
                        if (eph.IdProduct == pze.prize_id.id)
                            ex_events.Remove(eph);
                    }
                }

                DateTime date_last_ex = new DateTime(); DateTime date_avalible = new DateTime();
                if (ex_events.Count() > 0)
                {
                    
                    var latest_exch_event = ex_events.OrderBy(f => f.CreatedAt).Last();
                    date_last_ex = new DateTime(latest_exch_event.CreatedAt.Value.Year, latest_exch_event.CreatedAt.Value.Month, latest_exch_event.CreatedAt.Value.Day);
                    date_avalible = date_last_ex.AddMonths(1);
                    DateTime today = DateTime.Now;
                    today = new DateTime(today.Year, today.Month, today.Day);
                    // Difference in days, hours, and minutes.
                    TimeSpan ts = date_avalible - today;

                    // Difference in days.
                    int differenceInDays = ts.Days;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");

                    // set dias habilitado por prize
                    foreach (Prize_pz pz in allPrizes.data[0].prizes)
                    {
                       
                        if ( pz.prize_id.category.id != 7)
                        {
                            pz.prize_id.dias_habilitado = differenceInDays;
                        }
                    }

                    allPrizes.data[0].dias_habilitado = differenceInDays;
                    allPrizes.data[0].fecha_canjeo = dateString;

                    // set exchange_reward featured 
                    if (allPrizes.data[0].featured_prize[0].prize_id.category.id != 7)
                    {
                        allPrizes.data[0].featured_prize[0].prize_id.dias_habilitado = differenceInDays;
                    }

                }
                else
                {
                    allPrizes.data[0].dias_habilitado = 0;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");
                    allPrizes.data[0].fecha_canjeo = dateString;
                }
                response.Result = mapper.Map<PrizeResponse>(allPrizes);
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

        [HttpGet("GetPrizes")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> GetPrizes()
        {
            var response = new ApiResponse<PrizeResponse>();
            try
            {

                DataPrizes dataPrizes = new DataPrizes();
                var allPrizes = await dataPrizes.GetAllPrizes();
                response.Result = mapper.Map<PrizeResponse>(allPrizes);
                foreach (Prize_pz pz in allPrizes.data[0].prizes.ToList())
                {
                    var date = DateTime.Now;
                    //quitar cuando al fecha ya caduco
                    if (date.CompareTo(DateTime.Parse(pz.prize_id.finish_date)) > 0)
                    {
                        allPrizes.data[0].prizes.Remove(pz);
                    }

                    //habilitar si la fecha ya entro en vigor si no poner como próximamente
                    if (date.CompareTo(DateTime.Parse(pz.prize_id.start_date)) > 0)
                    {
                        pz.prize_id.habilitado = true;
                    }
                    else
                    {
                        pz.prize_id.habilitado = false;
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