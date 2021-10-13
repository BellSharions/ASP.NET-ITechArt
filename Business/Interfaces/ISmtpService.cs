using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISmtpService
    {
        public Task SendAsync(string to, string subject, string body);
    }
}
