using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Request;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext context;
        public NotificationController(IMapper mapper, Db_DestappContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("save_token")]
        public ActionResult<ApiResponse<SaveTokenResponse>> saveToken(SaveTokenRequest saveTokenRequest)
        {
            var response = new ApiResponse<SaveTokenResponse>();
            try
            {
                var datosUsuario = context.DatosUsuarios.Find(saveTokenRequest.idUser);
                if(datosUsuario == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ datosUsuario.IdDatoUsuario }' Not Found";
                    return NotFound(response);
                }
                datosUsuario.DeviceToken = saveTokenRequest.token;
                context.Entry(datosUsuario).Property(x => x.DeviceToken).IsModified = true;
                context.SaveChanges();
                response.Success = true;
                response.Message = "token guardado con exito";
                response.Result = new SaveTokenResponse()
                {
                    idUsuario = datosUsuario.IdDatoUsuario
                };
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return response;
        }

        [HttpPost("delete_token")]
        public ActionResult<ApiResponse<SaveTokenResponse>> deleteToken(SaveTokenRequest saveTokenRequest)
        {
            var response = new ApiResponse<SaveTokenResponse>();
            try
            {
                var datosUsuario = context.DatosUsuarios.Find(saveTokenRequest.idUser);
                if (datosUsuario == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ datosUsuario.IdDatoUsuario }' Not Found";
                    return NotFound(response);
                }
                datosUsuario.DeviceToken = null;
                context.Entry(datosUsuario).Property(x => x.DeviceToken).IsModified = true;
                context.SaveChanges();
                response.Success = true;
                response.Message = "token guardado con exito";
                response.Result = new SaveTokenResponse()
                {
                    idUsuario = datosUsuario.IdDatoUsuario
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return response;
        }

        //[HttpPost("{}")]
        [HttpGet("{idUsuario}/all")]
        public ActionResult<ApiResponse<List<Notification>>> getAllNotifications(string idUsuario)
        {
            var response = new ApiResponse<List<Notification>>();
            try
            {
                var datosUsuario = context.DatosUsuarios.Find(idUsuario);
                if (datosUsuario == null)
                {
                    response.Success = false;
                    response.Message = $"Usuario id '{ datosUsuario.IdDatoUsuario }' Not Found";
                    return NotFound(response);
                }
                var notificaciones = context.Notifications.Where(x => x.IdUser.Equals(idUsuario)).ToList();
                if(notificaciones.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Lista obtenida con éxito";
                    response.Result = notificaciones;
                }
                else
                {
                    response.Success = true;
                    response.Message = "Lista vacia";
                    response.Result = notificaciones;
                }
               
            }
            catch(Exception e)
            {

            }            
            return response;
        }

        [HttpGet("GetNotificationNotRead/{idUser}")]
        public async Task<ActionResult<ApiResponse<long>>> GetNotificationNotRead(string idUser)
        {
            var response = new ApiResponse<long?>();
            try
            {
                var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }
                var notificationNotRead = await (from n in context.Notifications where n.IdUser.Equals(idUser) && n.DateRead == null select n).CountAsync();
                response.Success = true;
                response.Result = mapper.Map<long>(notificationNotRead);
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

        [HttpPost("notificationReaded")]
        public ActionResult<ApiResponse<Notification>> updateToReaded(long idNotificacion)
        {
            var response = new ApiResponse<Notification>();
            var notificacion = context.Notifications.Find(idNotificacion);
            if(notificacion == null)
            {
                response.Success = false;
                response.Message = "La notificación no se encontro";
                return NotFound(response);
            }

            notificacion.DateRead = DateTime.Now;
            context.SaveChanges();
            response.Success = true;
            response.Result = notificacion;
            response.Message = "Notificacion actualizada con éxito";
            return response;

        }

        [HttpGet("AyudaMsj/{idUser}/{msj}")]
        public async Task<ActionResult<ApiResponse<long>>> AyudaMsj(string idUser, string msj)
        {
            var response = new ApiResponse<long?>();


            await context.AddAsync(new UsersMessage
            {
                IdDatoUsuario = idUser,
                Text = msj,
                CreatedAt = DateTime.Now
            });
            try
            {
                await context.SaveChangesAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
            }

            //try
            //{
            //    //var existUser = await context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUser)).FirstOrDefaultAsync();
            //    //if (existUser == null)
            //    //{
            //    //    response.Success = false;
            //    //    response.Message = $"User id { idUser } Not Found";
            //    //    return NotFound(response);
            //    //}
            //    var notificationNotRead = await (from n in context.Notifications where n.IdUser.Equals(idUser) && n.DateRead == null select n).CountAsync();
            //    response.Success = true;
            //   // response.Result = mapper.Map<long>(notificationNotRead);
            //}
            //catch (Exception ex)
            //{
            //    response.Result = null;
            //    response.Success = false;
            //    response.Message = string.Format("Internal server error: {0}", ex.Message);
            //    return StatusCode(500, response);
            //}
            return Ok(response);
        }

    }
}