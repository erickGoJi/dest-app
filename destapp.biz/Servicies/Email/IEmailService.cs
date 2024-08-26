using destapp.biz.Entities;

namespace destapp.biz.Servicies
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
}
