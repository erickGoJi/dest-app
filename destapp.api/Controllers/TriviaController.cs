﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Juegos;
using destapp.api.Models.Request;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.apiClient.Models.Trivia;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static destapp.api.Models.Response.RankingResponse;
using static destapp.apiClient.Models.GameResponse;
using static destapp.apiClient.Models.Trivia.Trivia;
using static destapp.apiClient.Models.Trivia.Question.Answer;
using static destapp.apiClient.Models.Trivia.TriviaResponse;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TriviaController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public TriviaController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{idUser}/{platform}/all")] /////////// se usa en producción a partir del releasse del 20 de Enero deja de usarse en el hot fix
        //[AllowAnonymous] // en la ap si se requiere
        public async Task<ActionResult<ApiResponse<TriviaResponse>>> GetAll(string idUser,string platform)
        {
            var response = new ApiResponse<TriviaResponse>();
            try
            {
                apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
                TriviaResponse triviaResponse = await triviaCms.getAllTrivias();
                var trivias_intentos = context.IntentosTrivia.Where(gr => gr.IdUsuario == idUser).GroupBy(tri => tri.IdTrivia).Select(group => new
                {
                    idtrivia = group.Key,
                    Count = group.Count()
                }).OrderBy(x => x.idtrivia).ToList();
                
                foreach (DataTrivia data in triviaResponse.data)
                {
                    foreach (TriviaPrivate tr in data.trivia.ToList())
                    {
                        // quita las que ya caducaron 
                        var date = DateTime.Now;
                        if (date.CompareTo(tr.trivia_id.end_date) > 0)
                        {
                            data.trivia.Remove(tr);
                            continue;
                        }
                       
                        // para no mostrar imagen scan en ios
                        if (!platform.Equals("android"))
                        {
                            if (tr.trivia_id.access_type.value.Equals("image"))
                            {
                                data.trivia.Remove(tr);
                                continue;
                            }
                        }

                        // setea los intentos disponibles
                        var trivia_intentos = trivias_intentos.Find(intento => intento.idtrivia == tr.trivia_id.id);
                        if(trivia_intentos != null)
                        {
                            if ((tr.trivia_id.times_per_user - trivia_intentos.Count) >= 0)
                                tr.trivia_id.intentos_disp = tr.trivia_id.times_per_user - trivia_intentos.Count;
                            else
                                tr.trivia_id.intentos_disp = 0;

                            if (data.featured_trivia[0].featured_trivia_id.id == tr.trivia_id.id)
                                data.featured_trivia[0].featured_trivia_id.intentos_disp = tr.trivia_id.intentos_disp;
                        }
                        else
                        {
                            tr.trivia_id.intentos_disp = tr.trivia_id.times_per_user;
                            if (data.featured_trivia[0].featured_trivia_id.id == tr.trivia_id.id)
                                data.featured_trivia[0].featured_trivia_id.intentos_disp = tr.trivia_id.intentos_disp;
                        }                        
                        

                        //set interes trivias
                        var interest_trivia = context.InteresesTrivias.Where(f => f.IdUser == idUser && f.IdTrivia == tr.trivia_id.id);
                        if(interest_trivia.Count() > 0)
                            tr.trivia_id.me_interesa = true;

                        // set días espera 
                        ManageDaysAvalible mandays = new ManageDaysAvalible(context);
                        if (tr.trivia_id.prize.Count > 0)
                        {
                            if (tr.trivia_id.prize[0].prize_id.category != null)
                                tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", tr.trivia_id.prize[0].prize_id.category.id);
                            else
                                tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", 0); // viene la categoria del premio nula
                        }  
                        else
                            tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", 0); // en este caso la trivia otorga ticket o coins

                        //habilitado o en próximamente 
                        if (date.CompareTo(tr.trivia_id.start_date) > 0)
                        {
                            tr.trivia_id.habilitado = true;
                        }
                        else
                        {
                            tr.trivia_id.habilitado = false;
                        }


                        //validar ganadores por día 
                        DateTime hoy_st = DateTime.Now;
                        var wins_hoy = context.IntentosTrivia.Where(f => f.FechaHora.Day == hoy_st.Day
                        && f.FechaHora.Month == hoy_st.Month
                        && f.FechaHora.Year == hoy_st.Year
                        && f.IdTrivia == tr.trivia_id.id
                        && f.Gano == true).ToList();
                        if (wins_hoy.Count() > 0)
                        {
                            int? intentos = 1000000;
                            if (tr.trivia_id.prize_per_day != null)
                                intentos = tr.trivia_id.prize_per_day;
                                                           
                            if (wins_hoy.Count() >= intentos)
                            {
                                data.trivia.Remove(tr);
                            }
                        }

                    }

                }

                int id_ft = triviaResponse.data[0].featured_trivia[0].featured_trivia_id.id;
                // set interes featured trivia
                var interest_ftrivia = context.InteresesTrivias.Where(f => f.IdUser == idUser && f.IdTrivia == id_ft);
                if (interest_ftrivia != null)
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.me_interesa = true;

                // intentos diponibles featured trivias
                var trivia_intentos_f = context.IntentosTrivia.Where(intento => intento.IdTrivia == id_ft && intento.IdUsuario == idUser);
                if (trivia_intentos_f != null)
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.intentos_disp = triviaResponse.data[0].featured_trivia[0].featured_trivia_id.times_per_user - trivia_intentos_f.Count();

                // set dias espera featured trivia
                ManageDaysAvalible mandays_ = new ManageDaysAvalible(context);
                if (triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize.Count > 0)
                {
                    if (triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize[0].prize_id.category != null)
                        triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize[0].prize_id.category.id);
                    else
                        triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", 0); // viene la categoria del premio nula
                }
                else
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", 0); // en este caso la trivia otorga ticket o coins

                //habilitado o en próximamente 
                var date_f = DateTime.Now;
                if (date_f.CompareTo(triviaResponse.data[0].featured_trivia[0].featured_trivia_id.start_date) > 0)
                {
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.habilitado = true;
                }
                else
                {
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.habilitado = false;
                }


                response.Result = mapper.Map<TriviaResponse>(triviaResponse);
            }catch(Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost("startMatch")]
        //[AllowAnonymous] // si lo esta enviando al app
        public ActionResult<ApiResponse<StartTriviaResponse>> StartMatchCoins(GameTriviaRequest GameTriviaRequest)
        {
            var response = new ApiResponse<StartTriviaResponse>();
            var user = this.context.DatosUsuarios.Find(GameTriviaRequest.idUsuario);

            if (user == null)
            {
                response.Success = false;
                response.Message = $"Usuario id '{ GameTriviaRequest.idUsuario }' Not Found";
                return NotFound(response);
            }

            var partida = new IntentosTrivium()
            {
                IdUsuario = GameTriviaRequest.idUsuario,
                IdTrivia = GameTriviaRequest.idTrivia,
                FechaHora = DateTime.Now,
                Intento = 1,
              //  IdCategoriaPrize = 0
            };
            context.Add(partida);
            context.SaveChanges();

            if(GameTriviaRequest.access_type == 4) // coins
            {

                ManageCoins manageCoins = new ManageCoins(context);
                var userCoinsResponse = manageCoins.manageCoins(GameTriviaRequest.idUsuario, (-1 * GameTriviaRequest.cantCoins), 2, partida.Id); //tipo 2 es trivias
                if (userCoinsResponse == null)
                {
                    //context.Remove(partida);
                    context.SaveChanges();
                    response.Success = false;
                    response.Message = $"El usuario '{user.NombreUsuario }' no cuenta con suficientes coins";
                    return Conflict(response);
                }
            }

            if (GameTriviaRequest.access_type == 2) // barcode
            {

            }

            if (GameTriviaRequest.access_type == 3) // imagen
            {

            }

            if (GameTriviaRequest.access_type == 1) // free
            {

            }


            response.Success = true;
            response.Message = "Partida iniciada con éxito";

            response.Result = new StartTriviaResponse()
            {
                idIntentoTrivia = partida.Id
            };
            return response;
        }

        [HttpPost("endMatch")] ////////////// se usa en producción a partir del releasse del 20 de Enero deja de usarse en el hot fix
        [AllowAnonymous] //aqui no se envia en la app
        public async Task<ActionResult<ApiResponse<EndTriviaMatchResponse>>> endMatchAsync(EndTriviaIntentoRequest EndTriviaIntentoRequest)
        {

            string last_operation = "cargar partida";
            var response = new ApiResponse<EndTriviaMatchResponse>();
            var partida = this.context.IntentosTrivia.Find(EndTriviaIntentoRequest.idIntentoTrivia);
            if (partida == null)
            {
                response.Success = false;
                response.Message = $"Partida '{ EndTriviaIntentoRequest.idIntentoTrivia }' No Encontrada";
                return NotFound(response);
            }
            try
            {
                // throw new Exception();
                var user = this.context.DatosUsuarios.Find(EndTriviaIntentoRequest.idUsuario);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ EndTriviaIntentoRequest.idUsuario }' Not Found";
                    return NotFound(response);
                }


                if (partida != null)
                {
                    partida.IdCategoriaPrize = EndTriviaIntentoRequest.idCategoria;
                    partida.Actualizado = DateTime.Now;

                    response.Result = new EndTriviaMatchResponse()
                    { idIntentoTrivia = partida.Id, win = false };

                    ///////////////////////////// Se valida que no gane mas de una vez la trivia //////////////////////////////////
                    last_operation = "get_wins";
                    var wins = this.context.IntentosTrivia.Where(f => f.IdUsuario == EndTriviaIntentoRequest.idUsuario
                                                                      && f.IdTrivia == EndTriviaIntentoRequest.idTrivia
                                                                      && f.Gano == true).ToList();
                    if (wins.Count > 0)
                    {
                        response.Success = true;
                        response.Result.win = false;
                        response.Message = $"¡Uy! <br> Ya ganaste en esta trivia. Inténtalo en otra.";
                        partida.Detalle = response.Message;
                       // partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }


                    ////////////// Se obtiene el detalle de la trivia desde el CMS
                    last_operation = "get_trivia_cms";
                    apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
                    TriviaIdResponse triviaResponse = await triviaCms.getAllTrivias_byid(EndTriviaIntentoRequest.idTrivia.ToString());

                    // terminar el proceso si la trivia aún no esta activa
                    var date = DateTime.Now;
                    if (DateTime.Parse(triviaResponse.data.start_date).CompareTo(date) > 0)
                    {
                        response.Success = false;
                        response.Result.win = false;
                        response.Message = $"Fecha de partida no valida, fecha y hora de inicio: " + triviaResponse.data.start_date.ToString();
                        partida.Detalle = response.Message + "Hora ejecución : " + DateTime.Now.ToString();
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }

                    // terminar el proceso si la trivia ya caducó
                    last_operation = "compare date";
                    if (date.CompareTo(DateTime.Parse(triviaResponse.data.end_date)) > 0)
                    {
                        response.Success = false;
                        response.Result.win = false;
                        response.Message = $"Fecha de partida no valida, la trivia caduco el : " + triviaResponse.data.start_date.ToString();
                        partida.Detalle = response.Message + "Hora ejecución : " + DateTime.Now.ToString();
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }

                    //////////////////////////// SE COMPARAN RESPUESTAS ///////////////////////////////////////
                    last_operation = "validate_answers";
                    ManageAnswers manage_answers = new ManageAnswers(context); // devulve las respuestas comparandolas con el CMS
                    List<TriviaIntentoRespuesta> TRUE_ANS = await manage_answers.get_respuestas_correctasAsync(EndTriviaIntentoRequest.answers, triviaResponse);

                    bool _lost = false;
                    List<RespuestasIntentoTrivium> LRI = new List<RespuestasIntentoTrivium>();
                    foreach (TriviaIntentoRespuesta Resp in TRUE_ANS)
                    {
                        RespuestasIntentoTrivium RInt = new RespuestasIntentoTrivium();
                        RInt.Correcta = Resp.status;
                        RInt.IdIntentoTrivia = EndTriviaIntentoRequest.idIntentoTrivia;
                        RInt.PreguntaId = Resp.id;
                        RInt.Respuesta = Resp.answer;
                        LRI.Add(RInt);
                        if (!Resp.status)
                            _lost = true;
                    }

                    //se agregan las respuestas al contexto
                    await context.RespuestasIntentoTrivia.AddRangeAsync(LRI);

                    if (!_lost)
                    {

                        ////////////////////////// SE REGISTRNA PREMIOS ///////////////////////////////////////////

                        if (EndTriviaIntentoRequest.prize_type == "prize")
                        {

                            ///////////////// TRAER NOTIFICACION DEL CMS ////////////////////////
                            last_operation = "get_notificacion_cms";
                            DataTypeNotification dataTypeNotification = new DataTypeNotification();
                            var dataNotification = await dataTypeNotification.GetTypeNotificationByName(EndTriviaIntentoRequest.notificationtype);

                            if (dataNotification == null || dataNotification.data.Count < 1)//error al traer la notificacion del cms
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"!Ups¡ Ocurrio un eror al procesar la partida.";
                                partida.Detalle = response.Message + "error al traer la notificacion del cms, no existe notificación :" + EndTriviaIntentoRequest.notificationtype + " last operation: " + last_operation;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }


                            /////////// DESCONTAR DEL STOCK-TRIVIA //////////////////////////////
                            last_operation = "discount_cms";
                            var discount = await DiscountStock.DiscountStockApiTrivia(EndTriviaIntentoRequest.idRecompensa, EndTriviaIntentoRequest.idTrivia);
                            if (discount == null) //error al descontar
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"Discount stock failed";
                                partida.Detalle = response.Message + " last operation: " + last_operation;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }
                            if (discount.message == "No stock") // ya no hay stock
                            {
                                response.Success = true;
                                response.Result.win = false;
                                response.Message = $"¡Ayñ, lo sentimos! <br>Tu respuesta fue correcta pero no fue la más rápida. ¡Ánimo!Sigue intentándolo." + " last operation: ";
                                partida.Detalle = response.Message + " last operation: " + last_operation;
                                partida.Gano = false;
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }


                            // Sólo llega hasta acá si no hubo ningun error previo

                            // se cargar la recompenza para validar la fecha
                            //DataInfoProduct dataInfoProduct = new DataInfoProduct();  // var CMSProduct = await dataInfoProduct.GetNameProduct(EndTriviaIntentoRequest.idRecompensa);
                            var CMSProduct = triviaResponse.data.prize[0];
                            last_operation = "set_notificacion_context";
                            //notificación y se guarda en el contexto
                            biz.Entities.Notification notification = new biz.Entities.Notification();
                            notification.CreatedAt = DateTime.Now;
                            notification.DateRead = null;
                            notification.Icon = dataNotification.data[0].icon == null ? "" : dataNotification.data[0].icon.data.full_url;
                            notification.IdUser = EndTriviaIntentoRequest.idUsuario;
                            notification.Text = dataNotification.data[0].description;
                            notification.Title = dataNotification.data[0].title;
                            notification.Type = EndTriviaIntentoRequest.notificationtype;
                            notification.Text = notification.Text.Replace("*|id|*", CMSProduct.prize_id.name);
                            notification.Title = notification.Title.Replace("*|id|*", CMSProduct.prize_id.name);
                            await context.Notifications.AddAsync(notification);

                            last_operation = "set_RecompensasTriviasHistorial_context";
                            //Historial de canje se guarda en el contexto
                            RecompensasTriviasHistorial eph = new RecompensasTriviasHistorial();
                            eph.IdUser = notification.IdUser;
                            eph.IdRecompensa = EndTriviaIntentoRequest.idRecompensa;
                            eph.IdIntentoTrivia = EndTriviaIntentoRequest.idIntentoTrivia;
                            eph.IdCategoria = EndTriviaIntentoRequest.idCategoria;
                            eph.CreatedAt = DateTime.Now;
                            await context.RecompensasTriviasHistorials.AddAsync(eph);

                            last_operation = "save_context";
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste #RECOMPENSA. Checa en tus notificaciones los detalles para recibirla.";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        if (EndTriviaIntentoRequest.prize_type == "coins")
                        {

                            ManageCoins manageCoins = new ManageCoins(context);
                            var userCoinsResponse = manageCoins.manageCoins(EndTriviaIntentoRequest.idUsuario, (EndTriviaIntentoRequest.amount), 2, partida.Id); //tipo 2 es trivias
                            if (userCoinsResponse == null)
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"Error al agregar los coins";
                                partida.Detalle = response.Message;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); //salva la partida y sus respuestas
                                return Conflict(response);
                            }

                            // Sólo llega hasta acá si no hubo ningun error previo
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste " + EndTriviaIntentoRequest.amount.ToString() + " Coins, .";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        if (EndTriviaIntentoRequest.prize_type == "tickets")
                        {
                            ManageTickets manageTickest = new ManageTickets(context);
                            var userTicketsResponse = manageTickest.manageTickets(EndTriviaIntentoRequest.idUsuario, (EndTriviaIntentoRequest.amount), 2, partida.Id); //tipo 2 es trivias
                            if (userTicketsResponse == null)
                            {
                                //  context.SaveChanges();
                                response.Success = false;
                                response.Message = $"Error al agregar los tickets";
                                partida.Detalle = response.Message;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return Conflict(response);
                            }
                            // Sólo llega hasta acá si no hubo ningun error previo
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste " + EndTriviaIntentoRequest.amount.ToString() + " Tickets.";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        return response;
                    }
                    else
                    {
                        response.Success = true;
                        response.Result.win = false;
                        response.Message = $"¡Uy! Tu respuesta fue incorrecta. <br>Ánimo, sigue participando por más premios en las demás trivias.";
                        partida.Detalle = response.Message;
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync();
                        return response;
                    }
                }
                else
                {
                    last_operation = "partida_noexiste";
                    response.Success = false;
                    response.Result.win = false;
                    response.Message = $"¡Ups! <br> La partida no existe, intenta de nuevo o ponte en contacto con el administrador.";
                    partida.Detalle = response.Message;
                    partida.Gano = false;
                    context.IntentosTrivia.Update(partida);
                    await context.SaveChangesAsync();
                    return response;
                }

            }
            catch (Exception ex)
            {

                var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
                foreach (var entry in changedEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }

                //     var _partida = this.context.IntentosTrivia.Find(EndTriviaIntentoRequest.idIntentoTrivia);
                partida.Actualizado = DateTime.Now;
                partida.Detalle = ex.Message + " last operation: " + last_operation;
                partida.Gano = false;
                partida.IdCategoriaPrize = EndTriviaIntentoRequest.idCategoria;
                context.IntentosTrivia.Update(partida);
                var coins = this.context.CoinsUserHistories.FirstOrDefault(i => i.Source == partida.Id);
                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, (-1 * coins.Coins), 2, partida.Id); //tipo 2 es trivias
                await context.SaveChangesAsync();

                var _response = new ApiResponse<EndTriviaMatchResponse>();
                _response.Result = new EndTriviaMatchResponse()
                { idIntentoTrivia = partida.Id, win = false };
                _response.Success = false;
                _response.Result.win = false;
                _response.Message = "Ocurrio un error al procesar la partida.";
                return _response;
                // return StatusCode(500, response);
            }

        }


        /////////////////////////// SERVICIOS PARA WEB ////////////////////////////////////////////////////

        [HttpPost("endMatchweb")]  /////////// LA USAN WEB Y APP a PARTIR DEL HTOFIX DEL 20 DE ENERO 2020
        [AllowAnonymous] //aqui no se envia en la app 
        public async Task<ActionResult<ApiResponse<EndTriviaMatchResponse>>> endMatchAsyncweb(EndTriviaIntentoRequest EndTriviaIntentoRequest)
        {

            string last_operation = "cargar partida";
            var response = new ApiResponse<EndTriviaMatchResponse>();
            var partida = this.context.IntentosTrivia.Find(EndTriviaIntentoRequest.idIntentoTrivia);
            if (partida == null)
            {
                response.Success = false;
                response.Message = $"Partida '{ EndTriviaIntentoRequest.idIntentoTrivia }' No Encontrada";
                return NotFound(response);
            }
            try
            {
              // throw new Exception();
                var user = this.context.DatosUsuarios.Find(EndTriviaIntentoRequest.idUsuario);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ EndTriviaIntentoRequest.idUsuario }' Not Found";
                    return NotFound(response);
                }

              
                if (partida != null)
                {
                    partida.IdCategoriaPrize = EndTriviaIntentoRequest.idCategoria;
                    partida.Actualizado = DateTime.Now;

                    response.Result = new EndTriviaMatchResponse()
                    { idIntentoTrivia = partida.Id, win = false };

                    ///////////////////////////// Se valida que no gane mas de una vez la trivia //////////////////////////////////
                    last_operation = "get_wins";
                    var wins = this.context.IntentosTrivia.Where(f => f.IdUsuario == EndTriviaIntentoRequest.idUsuario
                                                                      && f.IdTrivia == EndTriviaIntentoRequest.idTrivia
                                                                      && f.Gano == true).ToList();
                    if (wins.Count > 0)
                    {
                        response.Success = true;
                        response.Result.win = false;
                        response.Message = $"¡Uy! <br> Ya participaste en esta trivia. Inténtalo en otra.";
                        partida.Detalle = response.Message;
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }


                    ////////////// Se obtiene el detalle de la trivia desde el CMS
                    last_operation = "get_trivia_cms";
                    apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
                    TriviaIdResponse triviaResponse = await triviaCms.getAllTrivias_byid(EndTriviaIntentoRequest.idTrivia.ToString());

                    // terminar el proceso si la trivia aún no esta activa
                    var date = DateTime.Now;
                    if (DateTime.Parse(triviaResponse.data.start_date).CompareTo(date) > 0)
                    {
                        response.Success = false;
                        response.Result.win = false;
                        response.Message = $"Fecha de partida no valida, fecha y hora de inicio: " + triviaResponse.data.start_date.ToString();
                        partida.Detalle = response.Message + "Hora ejecución : " + DateTime.Now.ToString();
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }

                    // terminar el proceso si la trivia ya caducó
                    last_operation = "compare date";
                    if (date.CompareTo(DateTime.Parse(triviaResponse.data.end_date)) > 0)
                    {
                        response.Success = false;
                        response.Result.win = false;
                        response.Message = $"Fecha de partida no valida, la trivia caduco el : " + triviaResponse.data.start_date.ToString();
                        partida.Detalle = response.Message + "Hora ejecución : " + DateTime.Now.ToString();
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync(); // salva la partida y sus respuestas
                        return response;
                    }

                    //////////////////////////// SE COMPARAN RESPUESTAS ///////////////////////////////////////
                    last_operation = "validate_answers";
                    ManageAnswers manage_answers = new ManageAnswers(context); // devulve las respuestas comparandolas con el CMS
                    List<TriviaIntentoRespuesta> TRUE_ANS = await manage_answers.get_respuestas_correctasAsync(EndTriviaIntentoRequest.answers, triviaResponse);

                    bool _lost = false;
                    List<RespuestasIntentoTrivium> LRI = new List<RespuestasIntentoTrivium>();
                    foreach (TriviaIntentoRespuesta Resp in TRUE_ANS)
                    {
                        RespuestasIntentoTrivium RInt = new RespuestasIntentoTrivium();
                        RInt.Correcta = Resp.status;
                        RInt.IdIntentoTrivia = EndTriviaIntentoRequest.idIntentoTrivia;
                        RInt.PreguntaId = Resp.id;
                        RInt.Respuesta = Resp.answer;
                        LRI.Add(RInt);
                        if (!Resp.status)
                            _lost = true;
                    }

                    //se agregan las respuestas al contexto
                    await context.RespuestasIntentoTrivia.AddRangeAsync(LRI);

                    if (!_lost)
                    {

                        ////////////////////////// SE REGISTRNA PREMIOS ///////////////////////////////////////////

                        if (EndTriviaIntentoRequest.prize_type == "prize")
                        {

                            ///////////////// TRAER NOTIFICACION DEL CMS ////////////////////////
                            last_operation = "get_notificacion_cms";
                            DataTypeNotification dataTypeNotification = new DataTypeNotification();
                            var dataNotification = await dataTypeNotification.GetTypeNotificationByName(EndTriviaIntentoRequest.notificationtype);

                            if (dataNotification == null || dataNotification.data.Count < 1)//error al traer la notificacion del cms
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"!Ups¡ Ocurrio un eror al procesar la partida.";
                                partida.Detalle = response.Message + "error al traer la notificacion del cms, no existe notificación :" + EndTriviaIntentoRequest.notificationtype + " last operation: " + last_operation;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }


                            /////////// DESCONTAR DEL STOCK-TRIVIA //////////////////////////////
                            last_operation = "discount_cms";
                            var discount = await DiscountStock.DiscountStockApiTrivia(EndTriviaIntentoRequest.idRecompensa, EndTriviaIntentoRequest.idTrivia);
                            
                            if (discount == null) //error al descontar
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"Discount stock failed";
                                partida.Detalle = response.Message + " last operation: " + last_operation;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }
                            if (discount.message == "No stock") // ya no hay stock
                            {
                                response.Success = true;
                                response.Result.win = false;
                                response.Message = $"¡Ayñ, lo sentimos! <br>Tu respuesta fue correcta pero no fue la más rápida. ¡Ánimo!Sigue intentándolo." + " last operation: ";
                                partida.Detalle = response.Message + " last operation: " + last_operation;
                                partida.Gano = false;
                                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, Convert.ToInt32(triviaResponse.data.access_coins), 2, partida.Id); //tipo 2 es trivias

                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return response;
                            }


                            // Sólo llega hasta acá si no hubo ningun error previo

                            // se cargar la recompenza para validar la fecha
                            //DataInfoProduct dataInfoProduct = new DataInfoProduct();  // var CMSProduct = await dataInfoProduct.GetNameProduct(EndTriviaIntentoRequest.idRecompensa);
                            var CMSProduct = triviaResponse.data.prize[0];
                            last_operation = "set_notificacion_context";
                            //notificación y se guarda en el contexto
                            biz.Entities.Notification notification = new biz.Entities.Notification();
                            notification.CreatedAt = DateTime.Now;
                            notification.DateRead = null;
                            notification.Icon = dataNotification.data[0].icon == null ? "" : dataNotification.data[0].icon.data.full_url;
                            notification.IdUser = EndTriviaIntentoRequest.idUsuario;
                            notification.Text = dataNotification.data[0].description;
                            notification.Title = dataNotification.data[0].title;
                            notification.Type = EndTriviaIntentoRequest.notificationtype;
                            notification.Text = notification.Text.Replace("*|id|*", CMSProduct.prize_id.name);
                            notification.Title = notification.Title.Replace("*|id|*", CMSProduct.prize_id.name);
                            await context.Notifications.AddAsync(notification);

                            last_operation = "set_RecompensasTriviasHistorial_context";
                            //Historial de canje se guarda en el contexto
                            RecompensasTriviasHistorial eph = new RecompensasTriviasHistorial();
                            eph.IdUser = notification.IdUser;
                            eph.IdRecompensa = EndTriviaIntentoRequest.idRecompensa;
                            eph.IdIntentoTrivia = EndTriviaIntentoRequest.idIntentoTrivia;
                            eph.IdCategoria = EndTriviaIntentoRequest.idCategoria;
                            eph.CreatedAt = DateTime.Now;
                            await context.RecompensasTriviasHistorials.AddAsync(eph);

                            last_operation = "save_context";
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste #RECOMPENSA. Checa en tus notificaciones los detalles para recibirla.";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                          //  throw new Exception();
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        if (EndTriviaIntentoRequest.prize_type == "coins")
                        {

                            ManageCoins manageCoins = new ManageCoins(context);
                            var userCoinsResponse = manageCoins.manageCoins(EndTriviaIntentoRequest.idUsuario, (EndTriviaIntentoRequest.amount), 2, partida.Id); //tipo 2 es trivias
                            if (userCoinsResponse == null)
                            {
                                response.Success = false;
                                response.Result.win = false;
                                response.Message = $"Error al agregar los coins";
                                partida.Detalle = response.Message;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); //salva la partida y sus respuestas
                                return Conflict(response);
                            }

                            // Sólo llega hasta acá si no hubo ningun error previo
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste " + EndTriviaIntentoRequest.amount.ToString() + " Coins, .";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        if (EndTriviaIntentoRequest.prize_type == "tickets")
                        {
                            ManageTickets manageTickest = new ManageTickets(context);
                            var userTicketsResponse = manageTickest.manageTickets(EndTriviaIntentoRequest.idUsuario, (EndTriviaIntentoRequest.amount), 2, partida.Id); //tipo 2 es trivias
                            if (userTicketsResponse == null)
                            {
                                //  context.SaveChanges();
                                response.Success = false;
                                response.Message = $"Error al agregar los tickets";
                                partida.Detalle = response.Message;
                                partida.Gano = false;
                                context.IntentosTrivia.Update(partida);
                                await context.SaveChangesAsync(); // salva la partida y sus respuestas
                                return Conflict(response);
                            }
                            // Sólo llega hasta acá si no hubo ningun error previo
                            response.Success = true;
                            response.Result.win = true;
                            response.Message = $"¡Felicidades! <br>Contestaste correctamente y ganaste " + EndTriviaIntentoRequest.amount.ToString() + " Tickets.";
                            partida.Detalle = response.Message;
                            partida.Gano = true;
                            await context.SaveChangesAsync(); //guarda la partida y sus respuestas
                            return response;
                        }
                        return response;
                    }
                    else
                    {
                        response.Success = true;
                        response.Result.win = false;
                        response.Message = $"¡Uy! Tu respuesta fue incorrecta. <br>Ánimo, sigue participando por más premios en las demás trivias.";
                        partida.Detalle = response.Message;
                        partida.Gano = false;
                        context.IntentosTrivia.Update(partida);
                        await context.SaveChangesAsync();
                        return response;
                    }
                }
                else
                {
                    last_operation = "partida_noexiste";
                    response.Success = false;
                    response.Result.win = false;
                    response.Message = $"¡Ups! <br> La partida no existe, intenta de nuevo o ponte en contacto con el administrador.";
                    partida.Detalle = response.Message;
                    partida.Gano = false;
                    context.IntentosTrivia.Update(partida);
                    await context.SaveChangesAsync();
                    return response;
                }

            }
            catch (Exception ex)
            {

                var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
                foreach (var entry in changedEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }

           //     var _partida = this.context.IntentosTrivia.Find(EndTriviaIntentoRequest.idIntentoTrivia);
                partida.Actualizado = DateTime.Now;
                partida.Detalle = ex.Message + " last operation: " + last_operation ;
                partida.Gano = false;
                partida.IdCategoriaPrize = EndTriviaIntentoRequest.idCategoria;
                context.IntentosTrivia.Update(partida);
                var coins = this.context.CoinsUserHistories.FirstOrDefault(i => i.Source == partida.Id);
                ManageCoins manageCoins = new ManageCoins(context); //devolver coins
                manageCoins.manageCoins_nosave(EndTriviaIntentoRequest.idUsuario, (-1 * coins.Coins), 2, partida.Id); //tipo 2 es trivias
                await context.SaveChangesAsync();

                var _response = new ApiResponse<EndTriviaMatchResponse>();
                _response.Result = new EndTriviaMatchResponse()
                { idIntentoTrivia = partida.Id, win = false };
                _response.Success = false;
                _response.Result.win = false;
                _response.Message = "Ocurrio un error al procesar la partida.";
                return _response;
                // return StatusCode(500, response);
            }

        }


        [HttpGet("{idUser}/{platform}/allweb")]  /////////// LA USAN WEB Y APP a PARTIR DEL HTOFIX DEL 20 DE ENERO 2020
        //[AllowAnonymous]
        public async Task<ActionResult<ApiResponse<TriviaResponse>>> allweb(string idUser, string platform)
        {
            var response = new ApiResponse<TriviaResponse>();
            try
            {
                apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
                TriviaResponse triviaResponse = await triviaCms.getAllTrivias();
                var trivias_intentos = context.IntentosTrivia.Where(gr => gr.IdUsuario == idUser).GroupBy(tri => tri.IdTrivia).Select(group => new
                {
                    idtrivia = group.Key,
                    Count = group.Count()
                }).OrderBy(x => x.idtrivia).ToList();

                foreach (DataTrivia data in triviaResponse.data)
                {
                    foreach (TriviaPrivate tr in data.trivia.ToList())
                    {
                        var date = DateTime.Now;
                        if (date.CompareTo(tr.trivia_id.end_date) > 0)
                        {
                            data.trivia.Remove(tr);
                            continue;
                        }

                        if (date.CompareTo(tr.trivia_id.start_date) > 0)
                        {
                            tr.trivia_id.habilitado = true;
                        }
                        else
                        {
                            tr.trivia_id.habilitado = false;
                        }

                        if (!platform.Equals("android"))
                        {
                            if (tr.trivia_id.access_type.value.Equals("image"))
                            {
                                data.trivia.Remove(tr);
                                continue;
                            }

                        }

                        //set all answers correct = true 
                        foreach (QuestionData pregunta in tr.trivia_id.questions)
                        {
                            foreach (destapp.apiClient.Models.Trivia.Question.Answer ans in pregunta.question_id.answers)
                            {
                                ans.answer_id.correct_answer = true;
                            }
                        }

                        var trivia_intentos = trivias_intentos.Find(intento => intento.idtrivia == tr.trivia_id.id);

                        if (trivia_intentos != null)
                        {
                            if ((tr.trivia_id.times_per_user - trivia_intentos.Count) >= 0)
                                tr.trivia_id.intentos_disp = tr.trivia_id.times_per_user - trivia_intentos.Count;
                            else
                                tr.trivia_id.intentos_disp = 0;

                            if (data.featured_trivia[0].featured_trivia_id.id == tr.trivia_id.id)
                                data.featured_trivia[0].featured_trivia_id.intentos_disp = tr.trivia_id.intentos_disp;
                        }
                        else
                        {
                            tr.trivia_id.intentos_disp = tr.trivia_id.times_per_user;
                            if (data.featured_trivia[0].featured_trivia_id.id == tr.trivia_id.id)
                                data.featured_trivia[0].featured_trivia_id.intentos_disp = tr.trivia_id.intentos_disp;
                        }


                        //set interes trivias
                        var interest_trivia = context.InteresesTrivias.Where(f => f.IdUser == idUser && f.IdTrivia == tr.trivia_id.id);
                        if (interest_trivia.Count() > 0)
                            tr.trivia_id.me_interesa = true;

                        // set días espera 
                        ManageDaysAvalible mandays = new ManageDaysAvalible(context);
                        if (tr.trivia_id.prize.Count > 0)
                        {
                            if (tr.trivia_id.prize[0].prize_id.category != null)
                                tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", tr.trivia_id.prize[0].prize_id.category.id);
                            else
                                tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", 0); // viene la categoria del premio nula
                        }
                        else
                            tr.trivia_id.day_disp = mandays.get_dias_espera(idUser, tr.trivia_id.id, "trivias", 0); // en este caso la trivia otorga ticket o coins

                    }

                }

                int id_ft = triviaResponse.data[0].featured_trivia[0].featured_trivia_id.id;
                // set interes featured trivia
                var interest_ftrivia = context.InteresesTrivias.Where(f => f.IdUser == idUser && f.IdTrivia == id_ft);
                if (interest_ftrivia != null)
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.me_interesa = true;

                // intentos diponibles featured trivias
                var trivia_intentos_f = context.IntentosTrivia.Where(intento => intento.IdTrivia == id_ft && intento.IdUsuario == idUser);
                if (trivia_intentos_f != null)
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.intentos_disp = triviaResponse.data[0].featured_trivia[0].featured_trivia_id.times_per_user - trivia_intentos_f.Count();

                // set dias espera featured trivia
                ManageDaysAvalible mandays_ = new ManageDaysAvalible(context);
                if (triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize.Count > 0)
                {
                    if (triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize[0].prize_id.category != null)
                        triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", triviaResponse.data[0].featured_trivia[0].featured_trivia_id.prize[0].prize_id.category.id);
                    else
                        triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", 0); // viene la categoria del premio nula
                }
                else
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.day_disp = mandays_.get_dias_espera(idUser, id_ft, "trivias", 0); // en este caso la trivia otorga ticket o coins

                //set all answers correct = true 
                foreach (QuestionData pregunta_f in triviaResponse.data[0].featured_trivia[0].featured_trivia_id.questions)
                {
                    foreach (destapp.apiClient.Models.Trivia.Question.Answer ans in pregunta_f.question_id.answers)
                    {
                        ans.answer_id.correct_answer = true;
                    }
                }

                // definir si estan disponibles o próximamente 
                var date_f = DateTime.Now;
                if (date_f.CompareTo(triviaResponse.data[0].featured_trivia[0].featured_trivia_id.start_date) > 0)
                {
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.habilitado = true;
                }
                else
                {
                    triviaResponse.data[0].featured_trivia[0].featured_trivia_id.habilitado = false;
                }

                response.Result = mapper.Map<TriviaResponse>(triviaResponse);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                return StatusCode(500, response);
            }
            return Ok(response);
        }


        [HttpGet("GetAllNoUser")] /////////// LA USA LA WEB CUANDO NO ESTA LOGEADO EL USUARIO
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<GameResponse>>> GetAllNoUser()
        {
            var response = new ApiResponse<TriviaResponse>();
            try
            {
                apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
                TriviaResponse triviaResponse = await triviaCms.getAllTrivias();

                foreach (DataTrivia data in triviaResponse.data)
                {
                    foreach (TriviaPrivate tr in data.trivia.ToList())
                    {
                        var date = DateTime.Now;
                        if (date.CompareTo(tr.trivia_id.end_date) > 0)
                        {
                            data.trivia.Remove(tr);
                            continue;
                        }

                        if (date.CompareTo(tr.trivia_id.start_date) > 0)
                        { tr.trivia_id.habilitado = true; }
                        else
                        { tr.trivia_id.habilitado = false; }

                        //set all answers correct = true 
                        foreach (QuestionData pregunta in tr.trivia_id.questions)
                        {
                            foreach (destapp.apiClient.Models.Trivia.Question.Answer ans in pregunta.question_id.answers)
                            {
                                ans.answer_id.correct_answer = true;
                            }
                        }

                        //validar ganadores por día 
                        DateTime hoy_st = DateTime.Now;
                        var wins_hoy = context.IntentosTrivia.Where(f => f.FechaHora.Day == hoy_st.Day
                        && f.FechaHora.Month == hoy_st.Month
                        && f.FechaHora.Year == hoy_st.Year
                        && f.IdTrivia == tr.trivia_id.id
                        && f.Gano == true).ToList();
                        if (wins_hoy.Count() > 0)
                        {
                            int? intentos = 1000000;
                            if (tr.trivia_id.prize_per_day != null)
                                intentos = tr.trivia_id.prize_per_day;

                            if (wins_hoy.Count() >= intentos)
                            {
                                data.trivia.Remove(tr);
                            }
                        }
                    }

                }

                response.Result = mapper.Map<TriviaResponse>(triviaResponse);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }



    public class EndTriviaIntentoRequest
        {
            public int idIntentoTrivia { get; set; }
            public int idTrivia { get; set; }
            public string idUsuario { get; set; }
            public int idRecompensa { get; set; }
            public int idCategoria { get; set; }
            public string notificationtype { get; set; }
            public string  prize_type { get; set; }
            public int amount { get; set; }
            public List<TriviaIntentoRespuesta> answers { get; set; }
        }

        public class TriviaIntentoRespuesta
        {
            public int id { get; set; }
            public string answer { get; set; }
            public string kind { get; set; }
            public bool status { get; set; }
         }

        public class EndTriviaMatchResponse
        {
            public int idIntentoTrivia { get; set; }
            public bool win { get; set; }
        }


        public class StartTriviaResponse
        {
            public int idIntentoTrivia { get; set; }
        }

        public class GameTriviaRequest
        {
            public int idTrivia { get; set; }
            public string idUsuario { get; set; }
            public int cantCoins { get; set; }
            public int access_type { get; set; }
        }
}