using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Globalization;

namespace PocViseu.Api.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class HealthController : ControllerBase
    {

        private readonly WebControlDbContext _wcContext;
        private readonly IMailService _mailService;
        private readonly ILogSystemService _logSystemService;

        public HealthController(WebControlDbContext wcContext, IMailService mailService, ILogSystemService logSystemService)
        {
            this._wcContext = wcContext;
            this._mailService = mailService;
            this._logSystemService = logSystemService;  
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                _ = _logSystemService.Log("HealthController".ToLog("HealthCheck PING", LogLevelStatus.Low, 1, "PING"));
                return Ok($"{Environment.MachineName} on {System.Net.Dns.GetHostName()} has ok on {DateTime.Now.AddHours(SysConfig.TMZ).ToString()}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
