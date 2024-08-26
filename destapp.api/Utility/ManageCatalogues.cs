using destapp.apiClient.Models;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Utility
{
    public class ManageCatalogues
    {
        public static AvatarResponse AvatarSelectedByIdUser(AvatarResponse avatar, int? idAvatarUser)
        {
            avatar.data.ForEach(x =>
            {
                if (x.id == idAvatarUser)
                    x.isSelected = true;
            });
            return avatar;
        }

        public static InteresesResponse InterestSelectedByIdUser(InteresesResponse interest, List<GustosUsuario> gustos)
        {
            if (interest != null)
                interest.data.ForEach(x =>
                {
                    if (gustos != null)
                        gustos.ForEach(y =>
                        {
                            if (x.id == y.IdGusto)
                                x.isSelected = true;
                        });
                });
            return interest;
        }

        public static BadgesResponse BadgeSelectedByIdUser(BadgesResponse badges, List<LogrosUsuario> logrosUsuarios)
        {
            if (badges != null)
                badges.data.ForEach(x =>
                {
                    if (logrosUsuarios != null)
                        logrosUsuarios.ForEach(y =>
                        {
                            if (x.id == y.IdLogro)
                            {
                                x.isGetting = true;
                                x.isSelected = y.IsSelected;
                            }
                        });
                });
            return badges;
        }
    }
}
