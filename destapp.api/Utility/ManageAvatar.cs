using destapp.apiClient.CoreApiClient;
using destapp.apiClient.Models;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static destapp.apiClient.Models.AvatarResponse;

namespace destapp.api.Utility
{
    public class ManageAvatar
    {
        private readonly Db_DestappContext _context;
        public ManageAvatar(Db_DestappContext _context)
        {
            this._context = _context;
        }

        public async Task<Datum> GetAvatarByIdUser(string idUser)
        {
            DataAvatar dataAvatar = new DataAvatar();
            var allAvatars = await dataAvatar.GetAllAvatar();

            var avataresUser = await (from du in _context.DatosUsuarios
                                      where du.IdDatoUsuario.Equals(idUser)
                                      select du).FirstOrDefaultAsync();
            Datum avatar = new Datum();

            if (avataresUser != null)
                if (avataresUser.IdAvatar != null)
                    avatar = allAvatars.data.Find(x => x.id == avataresUser.IdAvatar);
            return avatar;
        }
    }
}
