using Newtonsoft.Json;
using System.Globalization;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Api.Services
{
    public class ServiceBot : IServiceBot
    {
        private readonly ILogger<ServiceBot> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpBot _httpBot;
        private readonly ILogSystemService _logSystemService;

        public ServiceBot(ILogger<ServiceBot> logger, IServiceScopeFactory scopeFactory, IHttpBot httpBot,
            ILogSystemService logSystemService)
        {
            this._logger = logger;
            this._scopeFactory = scopeFactory;
            this._httpBot = httpBot;
            this._logSystemService = logSystemService;
        }


        public Task CheckStartQueue()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _wcContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();

                    
                }
                  

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail error in CheckStartQueue: " + ex.ToString());
            }
            return Task.CompletedTask;
        }



        public Task CheckScheduling()
        {
            _ = _logSystemService.Log($"CheckScheduling...: {DateTimeOffset.Now}".ToLog("ServiceBot", LogLevelStatus.Low, 1));

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var wcContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();

                 

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail error in CheckScheduling: " + ex.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
