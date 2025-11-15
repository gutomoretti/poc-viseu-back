using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PocViseu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class CultureController : ControllerBase
    {

        private readonly WebControlDbContext _wcContext;

        public CultureController(WebControlDbContext wcContext)
        {
            this._wcContext = wcContext;
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult create([FromBody] PragueModelView register)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
                var findItem = _wcContext!.Culture!.FirstOrDefault(x => x.Descricao == register.Descricao && x.Excluido == false);
                if (findItem != null)
                    return BadRequest(new { error = true, data = "Já cadastrado!" });

                if (string.IsNullOrWhiteSpace(register.Descricao))
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                if (!register.Indice.HasValue)
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                Culture? item = new Culture();
                item.Descricao = register.Descricao;
                item.Indice = register.Indice;
                item.Codigo = register.Codigo;
                item.CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ);
                item.UserId = long.Parse(userId);
                _wcContext!.Add(item);
                _wcContext.SaveChanges();

                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpPost("update")]
        [Authorize(Roles = "admin")]
        public ActionResult update([FromBody] PragueModelView data)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
                var findItem = _wcContext.Culture.Single(x => x.Id == data.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "Nao localizada!" });

                if (string.IsNullOrWhiteSpace(data.Descricao))
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                if (!data.Indice.HasValue)
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                findItem.Descricao = data.Descricao;
                findItem.Indice = data.Indice;
                findItem.Codigo = data.Codigo;
                findItem.UpdatedAt = DateTime.Now.AddHours(SysConfig.TMZ);

                _wcContext.Update(findItem);

                _wcContext.SaveChanges();

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
                var findItem = _wcContext!.Culture!.Single(x => x.Id == identification.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "ao localizada!" });

                findItem!.UpdatedAt = DateTime.Now.AddHours(-3);
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
                var query = from p in _wcContext.Culture!.Where(x => x.Excluido == false)
                                     //.OrderByDescending(x => x.CreatedAt)
                                     .OrderBy(x => x.Indice)
                                    .ToList()
                                //where OrigemObraExt.Predicate(view).Invoke(p)
                            select p;
                return Ok(query);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpGet("listcombo")]
        [AllowAnonymous]
        public ActionResult listcombo()
        {
            try
            {
                var list = _wcContext.Culture!.Where(x => x.Excluido == false)
                                     .OrderBy(x => x.Indice)
                                     .Select(p => new ComboBoxView { Name = p.Descricao, Code = p.Id.ToString() });

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }
    }
}
