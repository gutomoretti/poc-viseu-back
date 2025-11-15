using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class PainelController : ControllerBase
    {
        private readonly WebControlDbContext _wcContext;
        private readonly ILogSystemService _logSystemService;

        public PainelController(WebControlDbContext wcContext, ILogSystemService logSystemService)
        {
            this._wcContext = wcContext;
            this._logSystemService = logSystemService;
        }



        [HttpGet("list")]
        //[Authorize(Roles = "admin")]
        public ActionResult List([FromQuery] FilterNameView? view)
        {
            try
            {
                var totalCompanies = 0;// _wcContext.Projects.Where(t => t.Excluido == false).Count();

                var totalProcess = _wcContext.Process.Where(t => t.Excluido == false).Count();

                var totalHistory = 0;

                //
                var query = from p in _wcContext.Process!.Where(x => x.Excluido == false)
                                   //.Include(companies => companies.Companies).ThenInclude(x => x.History)
                                   .OrderByDescending(x => x.CreatedAt)
                                   .Take(5)
                                   .ToList()
                            select p;

                List<object> topProcess = new List<object>();
                foreach (var item in query)
                {

                    var countWait = 0;// item.Projects.Where(x => x.Status == (int)ProcessStatus.Waiting).Count();
                    var countProcess = 0;// item.Projects.Where(x => x.Status != (int)ProcessStatus.Waiting).Count();
                    var percentage = Math.Round((double)(countProcess * 100) / (countWait + countProcess));

                    if (percentage == 0)
                        percentage = 0;

                    topProcess.Add(
                        new
                        {
                            item.Id,
                            item.HashId,
                            item.CreatedAt,
                            percentage = percentage.ToString().PadLeft(2, '0'),
                        }
                    );
                }


                ///
                Console.WriteLine("iniciando...");
                /// visits.Where(u => u.Start != null).GroupBy(u => u.Start.GetValueOrDefault().Hour).Select(g => new { OnHour = g.Key, Totals = g.Count() })


                var viewLog = new FilterLogSystemView();
                viewLog.DateInit = DateTime.Now.AddHours(SysConfig.TMZ).AddDays(-2).ToString("dd/MM/yyyy");
                viewLog.DateEnd = DateTime.Now.AddHours(SysConfig.TMZ).ToString("dd/MM/yyyy");

                var queryLog = _wcContext.LogSystem.Where(x => x.Excluido == false &&
                                      x.CreatedAt >= viewLog.DateInit!.ParseDate("00:00") && x.CreatedAt <= viewLog.DateEnd!.ParseDate("23:59"))
                                    .OrderByDescending(x => x.CreatedAt)
                                   .Take(1000)
                                   .ToList()
                                   .GroupBy(x => new { Hour = $"{x.CreatedAt.Day}/{x.CreatedAt.Month} {x.CreatedAt.Hour.ToString().PadLeft(2, '0')}", x.Level })
                                   .Select(o => new { o.Key.Level, o.Key.Hour, Total = o.Count() });


                List<string> labels = new List<string>();
                List<int> dataLogInfo = new List<int>();
                List<int> dataLogInfoCritical = new List<int>();
                int treatLabel = 0;
                foreach (var item in queryLog)
                {
                    treatLabel++;

                    if (treatLabel == 1)
                        labels.Add($"{item.Hour}hrs");
                    else
                        labels.Add("");

                    if (treatLabel == 3)
                        treatLabel = 0;



                    if (item.Level != (int)LogLevelStatus.High)
                        dataLogInfo.Add(item.Total);
                    else
                        dataLogInfo.Add(0);


                    if (item.Level == (int)LogLevelStatus.High)
                        dataLogInfoCritical.Add(item.Total * 3);
                    else
                        dataLogInfoCritical.Add(0);
                }

                labels.Reverse();
                dataLogInfo.Reverse();
                dataLogInfoCritical.Reverse();

                List<object> datasets = new List<object>();
                datasets.Add(new
                {
                    label = "Normales",
                    data = dataLogInfo.ToList(),
                    fill = false,
                    backgroundColor = "#00bb7e",
                    borderColor = "#00bb7e",
                    tension = 0.4
                });
                datasets.Add(new
                {
                    label = "Críticos",
                    data = dataLogInfoCritical.ToList(),
                    fill = false,
                    backgroundColor = "#C63737",
                    borderColor = "#C63737",
                    tension = 0.4
                });

                dynamic dataLog = new
                {
                    labels = labels.ToList(),
                    datasets = datasets.ToList(),
                };

                dynamic data = new
                {
                    totalCompanies,
                    totalProcess,
                    totalHistory,
                    topProcess,
                    dataLog
                };
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

    }
}
