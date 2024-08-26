using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageCoins
    {
        private readonly Db_DestappContext context;

        public ManageCoins(Db_DestappContext _context)
        {
            this.context = _context;
        }

        public dynamic manageCoins(string idUsuario, int cantCoins, int type, int source)
        {
            var userCoins = this.context.CoinsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if (userCoins == null)
            {
                userCoins = new CoinsUser();
                userCoins.IdUsuario = idUsuario;
                userCoins.CreatedAt = DateTime.Now;
                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Added;
            }
            else
            {

                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Modified;
            }
            if (cantCoins < 0)
            {
                if (userCoins.Coins < (cantCoins * -1))
                {
                    if (context.Entry(userCoins).State == (EntityState.Added))
                        context.SaveChanges();
                    //response.Success = false;
                    return null;
                }
            }
            var userCoinsLog = new CoinsUserHistory()
            {
                Coins = cantCoins,
                IdType = type,
                IdUsuario = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            context.Add(userCoinsLog);
            userCoins.Coins += cantCoins;
            context.SaveChanges();

            return new { userCoins, userCoinsLog };
        }

        public dynamic manageCoins_nosave(string idUsuario, int cantCoins, int type, int source)
        {
            var userCoins = this.context.CoinsUsers.Where(x => x.IdUsuario.Equals(idUsuario)).FirstOrDefault();
            if (userCoins == null)
            {
                userCoins = new CoinsUser();
                userCoins.IdUsuario = idUsuario;
                userCoins.CreatedAt = DateTime.Now;
                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Added;
            }
            else
            {

                userCoins.UpdatedAt = DateTime.Now;
                context.Entry(userCoins).State = EntityState.Modified;
            }
            if (cantCoins < 0)
            {
                if (userCoins.Coins < (cantCoins * -1))
                {
                    if (context.Entry(userCoins).State == (EntityState.Added))
                        //    context.SaveChanges();
                        //response.Success = false;
                        return null;
                }
            }
            var userCoinsLog = new CoinsUserHistory()
            {
                Coins = cantCoins,
                IdType = type,
                IdUsuario = idUsuario,
                Source = source,
                CreatedAt = DateTime.Now
            };
            context.Add(userCoinsLog);
            userCoins.Coins += cantCoins;
            //  context.SaveChanges();

            return new { userCoins, userCoinsLog };
        }


    }
}
