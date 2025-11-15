using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PocViseu.Core.Extensions;
using PocViseu.Infrastructure.Database;
using PocViseu.Infrastructure.Querys;
using PocViseu.Model.Config;
using PocViseu.Model.ModelView;
using PocViseu.Model.ModelView.User;

namespace WebControlCorp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/admin/users")]
    public class UserController : ControllerBase
    {
        private readonly WebControlDbContext _wcContext;


        public UserController(WebControlDbContext wcContext)
        {
            this._wcContext = wcContext;
        }

        [HttpGet("list")]
        // [Authorize(Roles = "admin")]
        public ActionResult List([FromQuery] FilterNameView? view)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value);
            try
            {
                object list;

                var user = _wcContext.Users!.Where(x => x.Id == userId)
                    .Include(perfil => perfil.Perfil).FirstOrDefault();
                if (user != null && user.Role != "admin")
                {

                    list = _wcContext.Users!.Where(x => x.Excluido == false && x.Id == userId)
                                         .Include(master => master.Perfil.Where(y => y.Excluido == false))
                                         .OrderByDescending(x => x.CreatedAt)
                                         .Take(1000);
                }
                else
                {

                    list = from p in _wcContext.Users!.Where(x => x.Excluido == false)
                                         .Include(master => master.Perfil.Where(y => y.Excluido == false))
                                         .OrderByDescending(x => x.CreatedAt)
                                         .ToList()
                           where UserExt.Predicate(view).Invoke(p)
                           select p;
                }


                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpPost("update")]
        // [Authorize(Roles = "admin")]
        public ActionResult update([FromBody] UpdateUser data)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value);
            try
            {
                var user = _wcContext.Users!.Where(x => x.Id == userId)
                    .Include(perfil => perfil.Perfil).FirstOrDefault();

                if (user != null && user.Role != "admin")
                    data.Id = userId;


                var findItem = _wcContext.Users!.Single(x => x.Id == data.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "Não localizado!" });

                if (string.IsNullOrWhiteSpace(data.NomeCompleto) || string.IsNullOrWhiteSpace(data.Email))
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                if (string.IsNullOrEmpty(data.Password) == false && data.Password.Length < 6)
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios, Senha 6 digitos!" });


                findItem.NomeCompleto = data.NomeCompleto;
                findItem.Email = data.Email;
                findItem.Role = data.Role;
                findItem.Document = data.Document;

                if (string.IsNullOrEmpty(data.Password) == false)
                {
                    findItem.Password = data.Password.ToHash();
                }

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
                var findItem = _wcContext!.Users!.Single(x => x.Id == identification.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "Não localizado!" });

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


        [HttpGet("listcombo")]
        public ActionResult Listcombo()
        {
            try
            {
                var list = _wcContext.Users!.Where(x => x.Excluido == false)
                                   .OrderBy(x => x.NomeCompleto)
                                   .Select(p => new ComboBoxView { Name = p.NomeCompleto, Code = p.Id.ToString(), Param = p.Email });


                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

    }
}
