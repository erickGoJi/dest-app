using destapp.biz.Entities;

namespace destapp.biz.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
    }
}
