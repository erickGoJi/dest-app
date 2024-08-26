using destapp.biz.Entities;
using destapp.biz.Repository;
using destapp.dal.db_context;
using CryptoHelper;
using System;

namespace destapp.dal.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(Db_DestappContext context) : base(context) { }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string hash, string password)
        {
            return Crypto.VerifyHashedPassword(hash, password);
        }

        public override User Add(User user)
        {
            user.Password = HashPassword(user.Password);
            return base.Add(user);
        }

        public override User Update(User user, object id)
        {
            user.UpdatedAt = DateTime.Now;
            user.UpdateBy = user.Id;
            return base.Update(user, id);
        }
    }
}
