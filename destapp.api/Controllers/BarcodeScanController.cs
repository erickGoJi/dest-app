using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using destapp.api.Models;
using destapp.dal.db_context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeScanController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;
        public BarcodeScanController(IMapper mapper, Db_DestappContext _context)
        {
            this.mapper = mapper;
            this._context = _context;
        }

        [HttpGet("BarcodeScanByIdBarcode/{idUser}/{idBarcode}/{numCoins}")]
        public async Task<ActionResult<ApiResponse<Dictionary<string, string>>>> BarcodeScanByIdBarcode(string idUser, string idBarcode, int numCoins)
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
                switch (idBarcode)
                {
                    case "7501086801015":
                        objPrueba.Add("detalle_respuesta", "Producto registrado correctamente");
                        response.Success = true;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;
                    case "521353153215":
                        objPrueba.Add("detalle_respuesta", "Algo salió mal, escanea de nuevo el producto");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;
                    case "123456":
                        objPrueba.Add("detalle_respuesta", "Producto escaneado previamente");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;

                    case "9178472768788":
                        objPrueba.Add("detalle_respuesta", "Producto escaneado previamente");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;
                    case "1009981724791133873600493865726566":
                        objPrueba.Add("detalle_respuesta", "Producto escaneado previamente");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;
                    default:
                        objPrueba.Add("detalle_respuesta", "La opcion que busca no existe");
                        response.Success = false;
                        response.Result = mapper.Map<Dictionary<string, string>>(objPrueba);
                        break;
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