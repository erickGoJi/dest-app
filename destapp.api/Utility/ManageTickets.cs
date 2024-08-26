using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageTickets
    {
        private readonly Db_DestappContext _context;

        public ManageTickets(Db_DestappContext _context)
        {
            this._context = _context;
        }

        public dynamic manageTickets(string idUsuario, int cantTickets, int type, int? source)
        {
            var userTickets = this._context.TicketsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if (userTickets == null)
            {
                userTickets = new TicketsUser();
                userTickets.IdUsuario = idUsuario;
                userTickets.CreatedAt = DateTime.Now;
                userTickets.UpdatedAt = DateTime.Now;
                _context.Entry(userTickets).State = EntityState.Added;
            }
            else
            {

                userTickets.UpdatedAt = DateTime.Now;
                _context.Entry(userTickets).State = EntityState.Modified;
            }
            if (cantTickets < 0)
            {
                if (userTickets.Tickets < (cantTickets * -1))
                {
                    if (_context.Entry(userTickets).State == (EntityState.Added))
                        _context.SaveChanges();
                    return null;
                }
            }
            var userTicketsLog = new TicketsUserHistory()
            {
                Tickets = cantTickets,
                IdType = type,
                IdUser = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            _context.Add(userTicketsLog);
            userTickets.Tickets += cantTickets;
            _context.SaveChanges();

            return new { userTickets, userTicketsLog };
        }

        public dynamic manageTickets_nosave(string idUsuario, int cantTickets, int type, int? source)
        {
            var userTickets = this._context.TicketsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if (userTickets == null)
            {
                userTickets = new TicketsUser();
                userTickets.IdUsuario = idUsuario;
                userTickets.CreatedAt = DateTime.Now;
                userTickets.UpdatedAt = DateTime.Now;
                _context.Entry(userTickets).State = EntityState.Added;
            }
            else
            {

                userTickets.UpdatedAt = DateTime.Now;
                _context.Entry(userTickets).State = EntityState.Modified;
            }
            if (cantTickets < 0)
            {
                if (userTickets.Tickets < (cantTickets * -1))
                {
                    //if (_context.Entry(userTickets).State == (EntityState.Added))
                    //    _context.SaveChanges();
                    return null;
                }
            }
            var userTicketsLog = new TicketsUserHistory()
            {
                Tickets = cantTickets,
                IdType = type,
                IdUser = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            _context.Add(userTicketsLog);
            userTickets.Tickets += cantTickets;
           // _context.SaveChanges();

            return new { userTickets, userTicketsLog };
        }
    }
}
