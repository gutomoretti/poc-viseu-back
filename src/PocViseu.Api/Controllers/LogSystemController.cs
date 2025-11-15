using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Infrastructure.Querys;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("/api/logsystem")]
    public class LogSystemController : ControllerBase
    {
        private readonly WebControlDbContext _wcContext;
        private readonly ILogSystemService _logSystemService;

        public LogSystemController(WebControlDbContext wcContext, ILogSystemService logSystemService)
        {
            this._wcContext = wcContext;
            this._logSystemService = logSystemService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult create([FromBody] LogSystemModelView register)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {

                _ = _logSystemService.Log(register.Value!.ToLog(register.Description!, (LogLevelStatus)register.Level!, int.Parse(userId!), register.TraceKey!));

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }


        [HttpGet("list")]
        [Authorize(Roles = "admin")]
        public ActionResult List([FromQuery] FilterLogSystemView? view)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(view.DateInit))
                {
                    view.DateInit = DateTime.Now.AddHours(SysConfig.TMZ).AddDays(-2).ToString("dd/MM/yyyy");
                    view.DateEnd = DateTime.Now.AddHours(SysConfig.TMZ).ToString("dd/MM/yyyy");
                }

                var query = from p in _wcContext.LogSystem!.Where(x => x.Excluido == false &&
                                      x.CreatedAt >= view.DateInit!.ParseDate("00:00") && x.CreatedAt <= view.DateEnd!.ParseDate("23:59"))
                                     //.OrderByDescending(x => x.CreatedAt)
                                     .OrderByDescending(x => x.Id)
                                    .ToList()
                            where LogSystemExt.Predicate(view).Invoke(p)
                            select p;
                return Ok(query);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

    }
}

