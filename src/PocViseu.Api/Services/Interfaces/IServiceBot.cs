namespace PocViseu.Api.Services.Interfaces
{
    public interface IServiceBot
    {
        public Task CheckScheduling();
        public Task CheckStartQueue();
    }
}
