using destapp.api.Models.Response;
using destapp.apiClient.CoreApiClient;
using destapp.biz.Entities;
using destapp.dal.db_context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageNotification
    {
        public static async Task<ExchangeProductAndPushNotificationResponse> SaveNotification(Db_DestappContext _context, Notification notification, ExchangeProductHistory exchange)
        {
            var response = new ExchangeProductAndPushNotificationResponse();
            response.notification = await SaveNotification(notification, exchange.IdProduct, _context);

            if (response.notification != null)
            {
                ExchangeProductHistory eph = new ExchangeProductHistory();
                eph.IdUser = notification.IdUser;
                eph.IdProduct = exchange.IdProduct;
                eph.ValueInTickets = exchange.ValueInTickets;
                eph.CreatedAt = DateTime.Now;

                await _context.ExchangeProductHistories.AddAsync(eph);
            }
            else
                throw new Exception("No se pudo guardar la notificación");
            return response;
        }

        //NO va a consultar de nuevo el producto 
        public static async Task<ExchangeProductAndPushNotificationResponse> SaveNotificationNS(Db_DestappContext _context, Notification notification, ExchangeProductHistory exchange, string name)
        {
            var response = new ExchangeProductAndPushNotificationResponse();
            response.notification = await SaveNotificationNS(notification, name, _context);

            if (response.notification == null)
                throw new Exception("No se pudo guardar la notificación");
            return response;
        }

        public static async Task<ExchangeProductAndPushNotificationResponse> SaveNotificationTrivia(Db_DestappContext _context, Notification notification, Controllers.EndTriviaIntentoRequest exchange)
        {
            var response = new ExchangeProductAndPushNotificationResponse();
            response.notification = await SaveNotification(notification, exchange.idRecompensa, _context);

            if (response.notification != null)
            {
                RecompensasTriviasHistorial eph = new RecompensasTriviasHistorial();
                eph.IdUser = notification.IdUser;
                eph.IdRecompensa = exchange.idRecompensa;
                eph.IdIntentoTrivia = exchange.idIntentoTrivia;
                eph.IdCategoria =  exchange.idCategoria;
               // eph.Categoria = exchange.idCategoria.ToString();
                eph.CreatedAt = DateTime.Now;

                await _context.RecompensasTriviasHistorials.AddAsync(eph);
              //  await _context.SaveChangesAsync();
            }
            else
                throw new Exception("No se pudo guardar la notificación");
            return response;
        }


        //Save notification
        public static async Task<Notification> SaveNotification(Notification notification, int idProduct, Db_DestappContext _context)
        {
            notification.Text = await DescriptionNotificationComplete(notification.Text, idProduct);
            notification.Title = await DescriptionNotificationComplete(notification.Title, idProduct);
            await _context.Notifications.AddAsync(notification);
            return notification;
        }

        //Save notification no serivicio
        public static async Task<Notification> SaveNotificationNS(Notification notification, string name, Db_DestappContext _context)
        {
            notification.Text = notification.Text.Replace("*|id|*", name);
            notification.Title = notification.Title.Replace("*|id|*", name);
            await _context.Notifications.AddAsync(notification);
            return notification;
        }

        private static async Task<string> DescriptionNotificationComplete(string textNotification, int idProduct)
        {
            DataInfoProduct dataInfoProduct = new DataInfoProduct();
            var nameProduct = await dataInfoProduct.GetNameProduct(idProduct);
            return textNotification.Replace("*|id|*", nameProduct.data.name);
        }

        public class nom_text_not
        {
            public string Text { get; set; }
            public string Title { get; set; }
        }
    }
}
