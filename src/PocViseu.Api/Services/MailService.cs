using Microsoft.Extensions.Logging;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Model.Bussines;

namespace PocViseu.Api.Services
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger)
        {
            _logger = logger;
        }

        public Task Send(string message, string subject, string to, Process process)
        {
            _logger.LogInformation("MailService stub message to {Recipient}: {Subject}", to, subject);
            return Task.CompletedTask;
        }
    }
}
