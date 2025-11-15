using PocViseu.Model.Bussines;

namespace PocViseu.Api.Services.Interfaces
{
    public interface IHttpBot
    {
        //public Task ExecuteAll(string period, string tracekey);
        public Task<bool> ExecuteSome(Process process, bool execAll, int schedulingPlanType = 0);
    }

    public class ExecuteSomeRespone
    {
        public bool Sucess { get; set; } = false;
        public string Data { get; set; } = string.Empty;
    }
}
