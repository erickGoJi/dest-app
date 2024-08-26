using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageBadges
    {
        private readonly Db_DestappContext context;

        public ManageBadges(Db_DestappContext _context)
        {
            this.context = _context;
        }

        public dynamic managebadges_brand(string idUsuario, int cantCoins,  int idBrand)
        {
            int IdLogro = 0;
            switch (idBrand)
            {
                case 8:
                     IdLogro = 9; //"Escanear Vitaminwater por primera vez.",
                    break;
                case 2:
                     IdLogro = 10;// "Escanear Sprite por primera vez.",
                    break;
                case 12:
                    IdLogro = 11; //"Escanear Santa Clara por primera vez.",
                    break;
                case 6:
                    IdLogro = 12; //"Escanear Powerade por primera vez.",
                    break;
                case 7:
                    IdLogro = 13; //"Escanear Mundet por primera vez.",
                    break;
                case 13:
                    IdLogro = 14; //"Escanear ISOLITE por primera vez.",
                    break;
                case 14:
                    IdLogro = 15; //"Escanear fuze tea por primera vez.",
                    break;
                case 4:
                    IdLogro = 16; //"Escanear Fresca por primera vez.",
                    break;
                case 9:
                    IdLogro = 19; //Escanear Del Valle por primera vez.
                    break;
                default:
                    IdLogro = -1;
                    break;
            }

            var logroUser = this.context.LogrosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUsuario) && x.IdLogro == IdLogro).FirstOrDefault();

            if (logroUser == null && IdLogro > -1)
            {
                logroUser = new LogrosUsuario();
                logroUser.IdDatoUsuario = idUsuario;
                logroUser.CreatedAt = DateTime.Now;
                logroUser.IdLogro = IdLogro;
                logroUser.IsSelected = false;
                context.Entry(logroUser).State = EntityState.Added;
            }


            context.SaveChanges();

            return new { logroUser };
        }

        public dynamic managebadges_events(string idUsuario, int cantCoins, int idEvent)
        {
            int IdLogro = 0;
            switch (idEvent)
            {
                case 1:
                    IdLogro = 17; //Participar en 1 torneo a lo largo de un año\n
                    break;
                case 2:
                    IdLogro = 18;//Entrar al app\/web por primera vez\n
                    break;
                case 3:
                    IdLogro = 20; //Compartir un torneo 1 torneo activo a lo largo de un año\ncase 1:
                    break;
                case 4:
                    IdLogro = 21; //Retar a 1 amigo en una semana *(solo es válido si el amigo acepta el reto)\n
                    break;
                case 5:
                    IdLogro = 22; //"Jugar 5 veces consecutivas minjuego 1\n",
                    break;
                default:
                    IdLogro = -1; // no se hace nada 
                    break;
            }

            var logroUser = this.context.LogrosUsuarios.Where(x => x.IdDatoUsuario.Equals(idUsuario) && x.IdLogro == IdLogro).FirstOrDefault();

            if (logroUser == null && IdLogro > -1)
            {
                logroUser = new LogrosUsuario();
                logroUser.IdDatoUsuario = idUsuario;
                //  logroUser.CreatedAt = DateTime.Now;
                logroUser.IdLogro = IdLogro;
                logroUser.IsSelected = false;
                context.Entry(logroUser).State = EntityState.Added;
            }


            context.SaveChanges();

            return new { logroUser };
        }

    }
}
