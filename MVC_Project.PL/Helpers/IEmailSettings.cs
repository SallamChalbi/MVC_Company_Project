using MVC_Project.DAL.Models;

namespace MVC_Project.PL.Helpers
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
