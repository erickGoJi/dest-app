using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageDaysAvalible
    {
        private readonly Db_DestappContext context;

        public ManageDaysAvalible(Db_DestappContext _context)
        {
            this.context = _context;
        }

        public  int get_dias_espera(string idUsuario, int idtrivia,  string seccion, int idcategoria)
        {
            int dias_espera = 0;
            if (idcategoria != 7)
            {
                switch (seccion)
                {
                    case "vitrina":
                        dias_espera = 0;
                        break;
                    case "trivias":
                        var win_events = context.IntentosTrivia.Where(f => f.IdUsuario == idUsuario 
                                                                            && f.Gano == true 
                                                                            && f.IdCategoriaPrize != 7);
                        DateTime date_last_ex = new DateTime(); DateTime date_avalible = new DateTime();
                        if (win_events.Count() > 0)
                        {
                            var latest_exch_event = win_events.OrderBy(f => f.FechaHora).Last();
                            date_last_ex = new DateTime(latest_exch_event.FechaHora.Year, latest_exch_event.FechaHora.Month, latest_exch_event.FechaHora.Day);
                            date_avalible = date_last_ex.AddMonths(1);
                            DateTime today = DateTime.Now;
                            today = new DateTime(today.Year, today.Month, today.Day);
                            // Difference in days, hours, and minutes.
                            TimeSpan ts = date_avalible - today;

                            // Difference in days.
                            int differenceInDays = ts.Days;
                            string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");

                            dias_espera = differenceInDays;

                        }
                        else
                        {
                            dias_espera = 0;
                            string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        break;
                    case "torneos":
                        dias_espera = 11;
                        break;

                    default:
                        dias_espera = -1;
                        break;
                }

            }
            else
            {
                DateTime date_last_ex = new DateTime(); DateTime date_avalible = new DateTime();
                var win_events = this.context.IntentosTrivia.Where(f => f.IdUsuario == idUsuario 
                                                                        && f.IdTrivia == idtrivia 
                                                                        &&  f.Gano == true).ToList();
               
                if (win_events.Count() > 0)
                {
                    var latest_exch_event = win_events.OrderBy(f => f.FechaHora).Last();
                    date_last_ex = new DateTime(latest_exch_event.FechaHora.Year, latest_exch_event.FechaHora.Month, latest_exch_event.FechaHora.Day);
                    date_avalible = date_last_ex.AddMonths(1);
                    DateTime today = DateTime.Now;
                    today = new DateTime(today.Year, today.Month, today.Day);
                    // Difference in days, hours, and minutes.
                    TimeSpan ts = date_avalible - today;

                    // Difference in days.
                    int differenceInDays = ts.Days;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");

                    dias_espera = differenceInDays;

                }
                else
                {
                    dias_espera = 0;
                    string dateString = date_last_ex.ToString("yyyy-MM-dd HH:mm:ss");
                }

                
            }
            return dias_espera;
        }

    }
}
