using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;

namespace PocViseu.Api.Services
{
    public class LogSystemService : ILogSystemService
    {
        private readonly IServiceScopeFactory _scopeFactory;


        public LogSystemService(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public Task Log(LogSystem data)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var wcContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();
                    wcContext.Add(data);                  

                    wcContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail in insert log check migrate db, if table exists: " + ex.ToString());
            }
            return Task.CompletedTask;
        }
    }
}
