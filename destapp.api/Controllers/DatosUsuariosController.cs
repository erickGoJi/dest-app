using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using destapp.biz.Entities;
using destapp.dal.db_context;
using destapp.api.Models;
using AutoMapper;
using destapp.api.Utility;

namespace destapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatosUsuariosController : ControllerBase
    {
        IMapper mapper;
        private readonly Db_DestappContext _context;

        public DatosUsuariosController(IMapper mapper, Db_DestappContext context)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/DatosUsuarios
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DatosUsuario>>>> GetDatosUsuarios()
        {
            var response = new ApiResponse<List<DatosUsuario>>();
            try
            {
                var logros = (from du in _context.DatosUsuarios
                             join lu in _context.LogrosUsuarios on du.IdDatoUsuario equals lu.IdDatoUsuario
                             select lu);
                var logrosResult = await logros.ToListAsync();

                var intereses = (from du in _context.DatosUsuarios
                              join gu in _context.GustosUsuarios on du.IdDatoUsuario equals gu.IdDatoUsuario
                              select gu);
                var interesesResult = await intereses.ToListAsync();

                var usuarios = from du in _context.DatosUsuarios select du;
                var usuariosResult = await usuarios.ToListAsync();

                logrosResult.ForEach(x =>
                {
                    if (x != null)
                        usuariosResult.ForEach(y =>
                        {
                            if (y.IdDatoUsuario == x.IdDatoUsuario)
                                y.LogrosUsuarios.Add(x);
                        });
                });

                interesesResult.ForEach(x =>
                {
                    if (x != null)
                        usuariosResult.ForEach(y =>
                        {
                            if (y.IdDatoUsuario.Equals(x.IdDatoUsuario))
                                y.GustosUsuarios.Add(x);
                        });
                });


                //var result = await _context.DatosUsuarios.ToListAsync();
                response.Success = true;
                response.Result = mapper.Map<List<DatosUsuario>>(usuariosResult);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
            //return await _context.DatosUsuarios.ToListAsync();
        }

        // GET: api/DatosUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DatosUsuario>>> GetDatosUsuario(string id)
        {
            var response = new ApiResponse<DatosUsuario>();
            try
            {
                var user = await _context.DatosUsuarios.FindAsync(id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"User id { id } Not Found";
                    return NotFound(response);
                }
                var logros = await (from du in _context.DatosUsuarios
                              join lu in _context.LogrosUsuarios on du.IdDatoUsuario equals lu.IdDatoUsuario
                              where lu.IdDatoUsuario.Equals(du.IdDatoUsuario)
                              select lu).ToListAsync();

                var intereses = await (from du in _context.DatosUsuarios
                                 join gu in _context.GustosUsuarios on du.IdDatoUsuario equals gu.IdDatoUsuario
                                 where du.IdDatoUsuario.Equals(id)
                                 select gu).ToListAsync();

                var ticketsUserHistory = await (from tu in _context.TicketsUserHistories where tu.IdUser.Equals(id) select tu).ToListAsync();

                var usuarios = await (from du in _context.DatosUsuarios where du.IdDatoUsuario.Equals(id) select du).FirstOrDefaultAsync();

                if (logros != null)
                    foreach (var logro in logros)
                    {
                        usuarios.LogrosUsuarios.Add(logro);
                    }
                if (intereses != null)
                    foreach (var interes in intereses)
                    {
                        usuarios.GustosUsuarios.Add(interes);
                    }
                if (ticketsUserHistory != null)
                    usuarios.TicketsUserHistories = ticketsUserHistory;

                response.Result = mapper.Map<DatosUsuario>(usuarios);
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

        // PUT: api/DatosUsuarios/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DatosUsuario>>> PutDatosUsuario(string id, DatosUsuario datosUsuario)
        {
            var response = new ApiResponse<DatosUsuario>();
            try
            {
                var existUser = await _context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(id)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    response.Success = false;
                    response.Message = $"User id { id } Not Found";
                    return NotFound(response);
                }

                var deleteLogros = await _context.LogrosUsuarios.Where(x => x.IdDatoUsuario.Equals(id)).ToListAsync();
                if (deleteLogros != null)
                {
                    _context.LogrosUsuarios.RemoveRange(deleteLogros);
                    _context.SaveChanges();
                }

                var deleteIntereses = await _context.GustosUsuarios.Where(x=> x.IdDatoUsuario.Equals(id)).ToListAsync();
                if (deleteIntereses != null)
                {
                    _context.GustosUsuarios.RemoveRange(deleteIntereses);
                    _context.SaveChanges();
                }

                var udu = (from x in _context.DatosUsuarios
                            where x.IdDatoUsuario == id
                            select x).First();
                
                udu.IdAvatar = datosUsuario.IdAvatar;
                udu.LogrosUsuarios = datosUsuario.LogrosUsuarios;
                udu.NombreUsuario = datosUsuario.NombreUsuario;
                udu.FechaNacimiento = datosUsuario.FechaNacimiento;
                udu.IdGender = datosUsuario.IdGender;
                udu.NombreApellidos = datosUsuario.NombreApellidos;
                udu.Direccion = datosUsuario.Direccion;
                udu.IdEstado = datosUsuario.IdEstado;
                udu.IdCity = datosUsuario.IdCity;
                udu.GustosUsuarios = datosUsuario.GustosUsuarios;
                udu.LogrosUsuarios = datosUsuario.LogrosUsuarios;
                udu.FirstTime = datosUsuario.FirstTime;
                udu.Municipality = datosUsuario.Municipality;
                await _context.SaveChangesAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.InnerException.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        // POST: api/DatosUsuarios
        [HttpPost]
        public async Task<ActionResult<ApiResponse<DatosUsuario>>> PostDatosUsuario(DatosUsuario datosUsuario)
        {
            var response = new ApiResponse<DatosUsuario>();
            try
            {
                var existUser = await _context.DatosUsuarios.Where(x => x.IdDatoUsuario.Equals(datosUsuario.IdDatoUsuario)).FirstOrDefaultAsync();
                if (existUser == null)
                {
                    _context.DatosUsuarios.Add(datosUsuario);
                    await _context.SaveChangesAsync();
                    ManageCoins manageCoins = new ManageCoins(_context);
                    manageCoins.manageCoins(datosUsuario.IdDatoUsuario, 200, 7, 0);
                    response.Success = true;
                    response.Result = mapper.Map<DatosUsuario>(datosUsuario);
                    
                }
                else
                {
                    response.Success = true;
                    response.Message = "El usuario ya existe";
                    response.Result = mapper.Map<DatosUsuario>(existUser);
                }
            }
            catch (DbUpdateException ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = string.Format("Internal server error: {0}", ex.InnerException.Message);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        // DELETE: api/DatosUsuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DatosUsuario>> DeleteDatosUsuario(string id)
        {
            var datosUsuario = await _context.DatosUsuarios.FindAsync(id);
            if (datosUsuario == null)
            {
                return NotFound();
            }

            _context.DatosUsuarios.Remove(datosUsuario);
            await _context.SaveChangesAsync();

            return datosUsuario;
        }

        private bool DatosUsuarioExists(string id)
        {
            return _context.DatosUsuarios.Any(e => e.IdDatoUsuario == id);
        }
    }
}
