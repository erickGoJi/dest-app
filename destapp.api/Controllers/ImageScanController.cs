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
using destapp.api.Utility;
using Microsoft.AspNetCore.Authorization;
using destapp.apiClient.Models;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageScanController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public ImageScanController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }
        

        [HttpGet("ImageScanDynamic/{idUser}/{idBrand}/{numCoins}/{typeEventScannimgid}/{idTarjet}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<Dictionary<string, string>>>> ImageScanDynamic(string idUser, int idBrand, int numCoins, int typeEventScannimgid, int idTarjet)
        {
            var response = new ApiResponse<Dictionary<string, string>>();
            Dictionary<string, string> objPrueba = new Dictionary<string, string>();
            try
            {
                var user = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(idUser) select u).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }

               
                var type_scann = await _context.TypeEventScannImgs.Where(x => x.Id.Equals(typeEventScannimgid)).FirstOrDefaultAsync();
                if (type_scann == null)
                {
                    objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                    response.Success = false;
                    response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                }

                var scann_events = _context.ScannImgHistories.Where(f => f.Idusr == idUser && f.IdTypeEventScannImg == typeEventScannimgid && f.IdBrandProduct == idBrand);
                DateTime nd = DateTime.Now;
                if (scann_events.Count() > 0)
                {
                    var latest_scann_event = scann_events.OrderBy(f => f.Date).Last();
                    nd = latest_scann_event.Date.AddDays(Convert.ToDouble(type_scann.WaitingDays));
                }
                
                nd = new DateTime(nd.Year, nd.Month, nd.Day);
                DateTime today = DateTime.Now;
                today = new DateTime(today.Year, today.Month, today.Day);
                if (nd <= today)
                {
                    await _context.ScannImgHistories.AddAsync(new ScannImgHistory
                    {
                        Date = DateTime.Now,
                        IdTypeEventScannImg = typeEventScannimgid,
                        CoinsNumber = numCoins,
                        Idusr = idUser,
                        IdBrandProduct = idBrand,
                        IdTarjet = idTarjet
                    });
                    try
                    {
                        await _context.SaveChangesAsync();
                        ManageCoins manageCoins = new ManageCoins(_context);
                        manageCoins.manageCoins(idUser, numCoins, 3, 0); // tipo canjeo //source 0 por qu eno viene de ningun jeugo ni  triva
                        objPrueba.Add("detalle_respuesta", "Tu producto se registró correctamente.");
                        response.Success = true;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    }
                    catch (Exception ex)
                    {
                        objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto.");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    }
                }
                else
                {
                    objPrueba.Add("detalle_respuesta", "Ya escaneaste esta presentación. Inténtalo de nuevo dentro de 24 hrs.");
                    response.Success = false;
                    response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
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


        [HttpGet("ScannBarcodeDynamic/{idUser}/{barcode}/{numCoins}/{typeEventScannimgid}/{idTarjet}/{idBrand}")]
        public async Task<ActionResult<ApiResponse<Dictionary<string, string>>>> ScannBarcodeDynamic(string idUser, string barcode, int numCoins, int typeEventScannimgid, int idTarjet, int idBrand)
        {
            var response = new ApiResponse<Dictionary<string, string>>();
            Dictionary<string, string> objPrueba = new Dictionary<string, string>();
            try
            {
                var user = await (from u in _context.DatosUsuarios where u.IdDatoUsuario.Equals(idUser) select u).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"User id { idUser } Not Found";
                    return NotFound(response);
                }

                //obtener el codigo desde el CMS
                apiClient.CoreApiClient.Barcode Barcodeclass = new apiClient.CoreApiClient.Barcode();
                BarcodeResponse BarcodeCMS = await Barcodeclass.get_barcode(barcode);

                //validar coins a agrgar y codigo de barras
                if (BarcodeCMS == null)
                {
                    objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                    response.Success = false;
                    response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    return response;
                }
                else
                {
                    if(BarcodeCMS.data.Count() > 0)
                    {
                        numCoins = BarcodeCMS.data[0].coins_to_add;
                        idBrand = BarcodeCMS.data[0].brand.id;
                    }
                    else
                    {
                        objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        return response;
                    }
                }


                var type_scann = await _context.TypeEventScannImgs.Where(x => x.Id.Equals(typeEventScannimgid)).FirstOrDefaultAsync();
                if (type_scann == null)
                {
                    objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                    response.Success = false;
                    response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    return response;
                }

                var scann_events = _context.ScannCodebarHistories.Where(f => f.Idusr == idUser && f.IdTypeEventScannImg == typeEventScannimgid && f.CodebarProduct == barcode);
                DateTime nd = DateTime.Now;
                if (scann_events.Count() > 0)
                {
                    var latest_scann_event = scann_events.OrderBy(f => f.Date).Last();
                    nd = latest_scann_event.Date.AddDays(Convert.ToDouble(type_scann.WaitingDays)); //normalmente es un días mas 
                }

                nd = new DateTime(nd.Year, nd.Month, nd.Day);
                DateTime today = DateTime.Now;
                today = new DateTime(today.Year, today.Month, today.Day);
                if (nd <= today)
                {
                    ScannCodebarHistory sch = new ScannCodebarHistory();

                    sch.Date = DateTime.Now;
                    sch.IdTypeEventScannImg = typeEventScannimgid;
                    sch.CoinsNumber = numCoins;
                    sch.Idusr = idUser;
                    sch.CodebarProduct = barcode;
                    sch.IdTarjet = idTarjet;
                    await _context.ScannCodebarHistories.AddAsync(sch);
                    try
                    {
                        await _context.SaveChangesAsync();
                        ManageCoins manageCoins = new ManageCoins(_context);
                        manageCoins.manageCoins(idUser, numCoins, 3, idTarjet); // tipo canjeo 3 //source = tarjet

                        ManageBadges ManageBadges = new ManageBadges(_context);
                        ManageBadges.managebadges_brand(idUser, numCoins, idBrand); // tipo canjeo //source 0 por qu eno viene de ningun jeugo ni  triva
                        objPrueba.Add("detalle_respuesta", "Tu producto se registró correctamente.");
                        response.Success = true;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    }
                    catch (Exception ex)
                    {
                        objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                    }
                }
                else
                {
                    objPrueba.Add("detalle_respuesta", "Ya escaneaste esta presentación. Inténtalo de nuevo dentro de 24 hrs.");
                    response.Success = false;
                    response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
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