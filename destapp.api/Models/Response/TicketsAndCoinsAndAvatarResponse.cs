using destapp.apiClient.Models;
using destapp.biz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static destapp.apiClient.Models.AvatarResponse;

namespace destapp.api.Models.Response
{
    public class TicketsAndCoinsAndAvatarResponse
    {
        public TicketsUser tickets;
        public CoinsUser coins;
        public Datum avatar;
        public DatosUsuario user;
        public JuegosUsuariosPartida score;
        public long notificationNotRead { get; set; }


        public TicketsAndCoinsAndAvatarResponse()
        {
            tickets = new TicketsUser();
            coins = new CoinsUser();
            avatar = new Datum();
            user = new DatosUsuario();
            score = null;
        }
    }
}
