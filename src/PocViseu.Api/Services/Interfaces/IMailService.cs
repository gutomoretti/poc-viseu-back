using PocViseu.Model.Bussines;

namespace PocViseu.Api.Services.Interfaces
{
    public interface IMailService
    {
        public Task Send(string message, string subject, string to, Process process);
    }
}
