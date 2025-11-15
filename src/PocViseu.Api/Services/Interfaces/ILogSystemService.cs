using PocViseu.Model.Bussines;

namespace PocViseu.Api.Services.Interfaces
{
    public interface ILogSystemService
    {
        public Task Log(LogSystem data);
    }
}
