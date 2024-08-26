using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using destapp.api.Models;
using destapp.api.Models.Request;
using destapp.api.Models.Response;
using destapp.api.Utility;
using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeProductController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;

        public ExchangeProductController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpPost("{isWeb?}")] //////////va a dejar de funsionar para la versiond e hot fix del 15 de enero 2020, se deja para que producción siga funcionando
        public async Task<ActionResult<ApiResponse<ExchangeProductAndPushNotificationResponse>>> PostExchangeProduct(ExchangeProductAndSendNotificationRequest productAndSendNotificationRequest, bool isWeb = false)
        {
            if (isWeb)
            {
                Console.WriteLine("is web");
            }
            else
            {
                Console.WriteLine("is not web");
            }


            //TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            string token = productAndSendNotificationRequest.dataSendNotification.deviceToken;
            int idProduct = productAndSendNotificationRequest.exchangeProductHistory.IdProduct;

            var response = new ApiResponse<ExchangeProductAndPushNotificationResponse>();
            try
            {

                // se cargar la recompenza para validar la fecha
                DataInfoProduct dataInfoProduct = new DataInfoProduct();
                var CMSProduct = await dataInfoProduct.GetNameProduct(idProduct);

                // terminar el proceso si la recompensa aún no esta activa
                var date = DateTime.Now;
                if (DateTime.Parse(CMSProduct.data.start_date).CompareTo(date) > 0)
                {
                    response.Success = false;
                    response.Message = $"Fecha de canje no valida!";
                    return Conflict(response);
                }

                // terminar el proceso si la recompensa aún no esta activa
                if (date.CompareTo(DateTime.Parse(CMSProduct.data.finish_date)) > 0)
                {
                    response.Success = false;
                    response.Message = $"Fecha de canje no valida!";
                    return Conflict(response);
                }

                
                var existUser = await _context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(productAndSendNotificationRequest.exchangeProductHistory.IdUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { productAndSendNotificationRequest.exchangeProductHistory.IdUser } Not Found";
                    return NotFound(response);
                }

                //validar qu eno haya ganado esta recompensa antes 
                var ex_product = _context.ExchangeProductHistories.Where(f => f.IdUser == productAndSendNotificationRequest.exchangeProductHistory.IdUser
                                                                       && f.IdProduct == productAndSendNotificationRequest.exchangeProductHistory.IdProduct).ToList();

                if (ex_product != null)
                {
                    if (ex_product.Count > 0)
                    {
                        response.Success = false;
                        response.Message = $"Esta recompensa ya fue canjeada";
                        return Conflict(response);
                    }
                }

                // validar que no pueda canjear otra recompensa si no han pasado 30 días y no es especial 
                //DateTime menos_un_mes = DateTime.Now.AddMonths(-1);

                //var ex_events = _context.ExchangeProductHistories.Where(f => f.IdUser == productAndSendNotificationRequest.exchangeProductHistory.IdUser
                //                                                    && menos_un_mes.CompareTo(f.CreatedAt) > 0 ).ToList();
                //foreach (ExchangeProductHistory ex_e in ex_events)
                //{
                //    // se cargar la recompenza para validar la categoria
                //    DataInfoProduct dataInfoProduct_CAT = new DataInfoProduct();
                //    var CMSProduct_CAT = await dataInfoProduct_CAT.GetNameProduct(idProduct);
                //    if(CMSProduct_CAT.data.category.id != 7)
                //    {
                //        response.Success = false;
                //        response.Message = $"Ya canjeaste una recompensa hace menos de un mes.";
                //        return Conflict(response);
                //    }
                //}


                var discount = await DiscountStock.DiscountStockApi(productAndSendNotificationRequest.exchangeProductHistory.IdProduct);
                if(discount == null)
                {
                    response.Success = false;
                    response.Message = $"Discount stock failed";
                    return Conflict(response);
                }

                if (discount.message == "No stock")
                {
                    response.Success = false;
                    response.Message = discount.message;
                    return Conflict(response);
                }

                ManageTickets manageTickets = new ManageTickets(_context);
                var result = manageTickets.manageTickets(productAndSendNotificationRequest.exchangeProductHistory.IdUser, productAndSendNotificationRequest.exchangeProductHistory.ValueInTickets, 6, null);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = $"El usuario '{productAndSendNotificationRequest.exchangeProductHistory.IdUser }' no cuenta con suficientes tickets";
                    return Conflict(response);
                }

                DataTypeNotification dataTypeNotification = new DataTypeNotification();
                var dataNotification = await dataTypeNotification.GetTypeNotificationByName(productAndSendNotificationRequest.dataSendNotification.typeNotification);

                if(dataNotification == null)
                {
                    response.Success = false;
                    response.Message = $"No se encontraron catálogos de tipos de notificaión";
                    return Conflict(response);
                }

                biz.Entities.Notification notification = new biz.Entities.Notification();
                notification.CreatedAt = DateTime.Now;
                notification.DateRead = null;
                notification.Icon = dataNotification.data[0].icon == null? "" : dataNotification.data[0].icon.data.full_url ;
                notification.IdUser = productAndSendNotificationRequest.exchangeProductHistory.IdUser;
                notification.Text = dataNotification.data[0].description;
                notification.Title = dataNotification.data[0].title;
                notification.Type = productAndSendNotificationRequest.dataSendNotification.typeNotification;

                //Historial de canje se guarda en el contexto
                ExchangeProductHistory eph = new ExchangeProductHistory();
                eph.IdUser = productAndSendNotificationRequest.exchangeProductHistory.IdUser;
                eph.IdProduct = productAndSendNotificationRequest.exchangeProductHistory.IdProduct;
                eph.ValueInTickets = productAndSendNotificationRequest.exchangeProductHistory.ValueInTickets;
                eph.CreatedAt = DateTime.Now;
                await _context.ExchangeProductHistories.AddAsync(eph);

                var epapn = await ManageNotification.SaveNotificationNS(_context, notification, productAndSendNotificationRequest.exchangeProductHistory, CMSProduct.data.name);
                if(epapn == null)
                {
                    response.Success = false;
                    response.Message = $"No se pudo guardar la notificación en la tabla";
                    return Conflict(response);
                }

                await _context.SaveChangesAsync();
                epapn.exchangeProductHistory = productAndSendNotificationRequest.exchangeProductHistory;
                response.Success = true;
                response.Result = mapper.Map<ExchangeProductAndPushNotificationResponse>(epapn);
                //ts.Complete();
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}.", ex.Message);
                //ts.Dispose();
                return StatusCode(500, response);
            }
            return Ok(response);
        }


        [HttpPost("ExchangeProduct_new/{isWeb?}")] ////////// Entra en uso con la versión fix del 15 de enero
        public async Task<ActionResult<ApiResponse<ExchangeProductAndPushNotificationResponse>>> ExchangeProduct_new(ExchangeProductAndSendNotificationRequest productAndSendNotificationRequest, bool isWeb = false)
        {
            if (isWeb)
            {
                Console.WriteLine("is web");
            }
            else
            {
                Console.WriteLine("is not web");
            }


            //TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            string token = productAndSendNotificationRequest.dataSendNotification.deviceToken;
            int idProduct = productAndSendNotificationRequest.exchangeProductHistory.IdProduct;
            //using (var transaction = _context.Database.BeginTransaction())
            //{
            var response = new ApiResponse<ExchangeProductAndPushNotificationResponse>();
            try
            {

                // se cargar la recompenza para validar la fecha
                DataInfoProduct dataInfoProduct = new DataInfoProduct();
                var CMSProduct = await dataInfoProduct.GetNameProduct(idProduct);

                // terminar el proceso si la recompensa aún no esta activa
                var date = DateTime.Now;
                if (DateTime.Parse(CMSProduct.data.start_date).CompareTo(date) > 0)
                {
                    response.Success = false;
                    response.Message = $"Fecha de canje no valida!";
                    return response;
                }

                // terminar el proceso si la recompensa ya caducó
                if (date.CompareTo(DateTime.Parse(CMSProduct.data.finish_date)) > 0)
                {
                    response.Success = false;
                    response.Message = $"Fecha de canje no valida!";
                    return response;
                }

                //se valida si el usuario existe
                var existUser = await _context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(productAndSendNotificationRequest.exchangeProductHistory.IdUser)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { productAndSendNotificationRequest.exchangeProductHistory.IdUser } Not Found";
                    return response;
                }

                //validar que no haya ganado esta recompensa antes 
                var ex_product = _context.ExchangeProductHistories.Where(f => f.IdUser == productAndSendNotificationRequest.exchangeProductHistory.IdUser
                                                                       && f.IdProduct == productAndSendNotificationRequest.exchangeProductHistory.IdProduct).ToList();

                if (ex_product != null)
                {
                    if (ex_product.Count > 0)
                    {
                        response.Success = false;
                        response.Message = $"Esta recompensa ya fue canjeada previamente.";
                        return response;
                    }
                }

                //validar que no pueda canjear otra recompensa si no han pasado 30 días y no es especial
                DateTime one_month_ago = DateTime.Now.AddMonths(-1);

                var ex_events = _context.ExchangeProductHistories.Where(f => f.IdUser == productAndSendNotificationRequest.exchangeProductHistory.IdUser
                                                                    && one_month_ago.CompareTo(f.CreatedAt) < 0).ToList();

                if (ex_events != null)
                {
                    if (ex_events.Count > 0)
                    {
                        foreach (ExchangeProductHistory ex_e in ex_events)
                        {
                            // se cargar la recompenza para validar la categoria
                            DataInfoProduct dataInfoProduct_CAT = new DataInfoProduct();
                            var CMSProduct_CAT = await dataInfoProduct_CAT.GetNameProduct(idProduct);
                            if (CMSProduct_CAT.data.category.id != 7)
                            {
                                response.Success = false;
                                response.Message = $"!Ups¡ Canjeaste una recompensa hace menos de un mes.";
                                return response;
                            }
                        }
                    }
                }


                //se descuenta el número de tickets 
                ManageTickets manageTickets = new ManageTickets(_context);
                var result = manageTickets.manageTickets_nosave(productAndSendNotificationRequest.exchangeProductHistory.IdUser, productAndSendNotificationRequest.exchangeProductHistory.ValueInTickets, 6, null);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = $"¡Ups! <br> No cuentas con suficientes tickets";
                    return response;
                }

                //se obtiene el tipo de notificación y se guarda en la BD 
                DataTypeNotification dataTypeNotification = new DataTypeNotification();
                var dataNotification = await dataTypeNotification.GetTypeNotificationByName(productAndSendNotificationRequest.dataSendNotification.typeNotification);

                //se valdia la obtención de la notificación desde el CMS
                if (dataNotification == null)
                {
                    response.Success = false;
                    response.Message = $"No se encontraron catálogos de tipos de notificaión";
                    return response;
                }

                //notificación y se guarda en el contexto
                biz.Entities.Notification notification = new biz.Entities.Notification();
                notification.CreatedAt = DateTime.Now;
                notification.DateRead = null;
                notification.Icon = dataNotification.data[0].icon == null ? "" : dataNotification.data[0].icon.data.full_url;
                notification.IdUser = productAndSendNotificationRequest.exchangeProductHistory.IdUser;
                notification.Text = dataNotification.data[0].description;
                notification.Title = dataNotification.data[0].title;
                notification.Type = productAndSendNotificationRequest.dataSendNotification.typeNotification;
                notification.Text = notification.Text.Replace("*|id|*", CMSProduct.data.name);
                notification.Title = notification.Title.Replace("*|id|*", CMSProduct.data.name);
                await _context.Notifications.AddAsync(notification);

                //Historial de canje se guarda en el contexto
                ExchangeProductHistory eph = new ExchangeProductHistory();
                eph.IdUser = notification.IdUser;
                eph.IdProduct = productAndSendNotificationRequest.exchangeProductHistory.IdProduct;
                eph.ValueInTickets = productAndSendNotificationRequest.exchangeProductHistory.ValueInTickets;
                eph.CreatedAt = DateTime.Now;
                await _context.ExchangeProductHistories.AddAsync(eph);

                //se descuenta del stock 
                var discount = await DiscountStock.DiscountStockApi(productAndSendNotificationRequest.exchangeProductHistory.IdProduct);
                if (discount == null)
                {
                    response.Success = false;
                    response.Message = $"Discount stock failed";
                    // se deshacen los cambios en el contexto
                    return response;
                }
                // se valida que no haya errores al descontar del stock del  CMS
                if (discount.message == "No stock")
                {
                    response.Success = false;
                    response.Message = "¡Ayñ, lo sentimos! Se nos terminó este premio. Vuelve a la vitrina y checa qué más puedes canjear.";
                    // se deshacen los cambios en el contexto
                    return response;

                }

                // se guarda el contexto en la BD
                await _context.SaveChangesAsync();
                response.Message = "Ya puedes disfrutar de tu(s) " + CMSProduct.data.name + ". <br /> En tus notificaciones consulta los detalles de la entrega.";
                response.Success = true;
                //var rs = new ExchangeProductAndPushNotificationResponse(); 
                // response.Result.notification = mapper.Map<Notification>(notification);

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}.", ex.Message);
                return response;
            }
            return Ok(response);
            //}
        }

    }
}