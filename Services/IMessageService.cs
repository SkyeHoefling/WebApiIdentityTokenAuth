using System.Threading.Tasks;

namespace WebApiIdentityTokenAuth
{
    public interface IMessageService
    {
        Task Send(string email, string subject, string message);
    }    
}