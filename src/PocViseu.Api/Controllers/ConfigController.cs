using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PocViseu.Infrastructure.Database;
using PocViseu.Infrastructure.Querys;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly WebControlDbContext _wcContext;

        public ConfigController(WebControlDbContext wcContext)
        {
            this._wcContext = wcContext;
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult create([FromBody] WebcorpConfigModelView register)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
                var findItem = _wcContext!.WebcorpConfig!.FirstOrDefault(x => x.ParamKey == register.ParamKey && x.Excluido == false);
                if (findItem != null)
                    return BadRequest(new { error = true, data = "Ja existe, Cadastrado!" });


                WebcorpConfig? item = register.ToDataModel(userId!);
                _wcContext!.Add(item);
                _wcContext.SaveChanges();

                if (register.ParamKey == "SYSTEM_TIME_ZONE")
                {
                    SysConfig.TMZ = int.Parse(register!.ParamValue ?? "0");
                }

                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpPost("update")]
        [Authorize(Roles = "admin")]
        public ActionResult update([FromBody] WebcorpConfigModelView data)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
                var findItem = _wcContext.WebcorpConfig.Single(x => x.Id == data.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "nao localizado!" });

                if (string.IsNullOrWhiteSpace(data.ParamKey))
                    return BadRequest(new { error = true, data = "Verifique campos obrigat�rios!" });

                findItem.ParamKey = data.ParamKey;
                findItem.ParamValue = data.ParamValue;
                findItem.ParamDesc = data.ParamDesc;
                findItem.UpdatedAt = DateTime.Now.AddHours(SysConfig.TMZ);

                _wcContext.Update(findItem);

                _wcContext.SaveChanges();

                if (data.ParamKey == "SYSTEM_TIME_ZONE")
                {
                    SysConfig.TMZ = int.Parse(data!.ParamValue ?? "0");
                }

                return Ok(findItem);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpPut("delete")]
        [Authorize(Roles = "admin")]
        public ActionResult delete([FromBody] IdentificationView identification)
        {
            try
            {
                var findItem = _wcContext!.WebcorpConfig!.Single(x => x.Id == identification.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "nao localizado!" });

                findItem!.UpdatedAt = DateTime.Now.AddHours(SysConfig.TMZ);
                findItem!.Excluido = true;

                _wcContext.Update(findItem);

                _wcContext.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpGet("list")]
        [Authorize(Roles = "admin")]
        public ActionResult List([FromQuery] FilterNameView? view)
        {
            try
            {
                var query = from p in _wcContext.WebcorpConfig!.Where(x => x.Excluido == false)
                                     //.OrderByDescending(x => x.CreatedAt)
                                     .OrderBy(x => x.Id)
                                    .ToList()
                            where WebcorpConfigExt.Predicate(view).Invoke(p)
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
