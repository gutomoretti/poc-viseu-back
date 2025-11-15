using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PocViseu.Api.Jwt.Interfaces;
using PocViseu.Api.Jwt.Model;
using PocViseu.Core.Extensions;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Auth;
using PocViseu.Model.Config;
using PocViseu.Model.ModelView.User;

namespace PocViseu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;
        private readonly WebControlDbContext _wcDbContext;


        public AuthController(IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher, WebControlDbContext dbContext)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
            this._wcDbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.PrimarySid)?.Value;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"You UserId is {userId} name {username} on {DateTime.Now.AddHours(SysConfig.TMZ).ToString()}");
        }

        [AllowAnonymous]
        [HttpPost("authenticateemail")]
        public async Task<IActionResult> AuthenticateEmail([FromBody] UserCredential userCred)
        {
            //var user = _wcDbContext.Users.FirstOrDefault(x => x.Username == userCred.Username && x.Excluido == false);
            var user = _wcDbContext.Users.FirstOrDefault(x => x.Email == userCred.Username && x.Excluido == false);

            if (user != null && userCred.Password.ToVerify(user.Password!))
                return Ok(jWTAuthenticationManager.Authenticate(user));

            return Unauthorized();
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserCredential userCred)
        {
            var user = _wcDbContext.Users.FirstOrDefault(x => x.Username == userCred.Username && x.Excluido == false);
            //var user = _wcDbContext.Users.FirstOrDefault(x => x.Email == userCred.Username && x.Excluido == false);

            if (user != null && userCred.Password.ToVerify(user.Password!))
                return Ok(jWTAuthenticationManager.Authenticate(user));

            return Unauthorized();
        }

        [HttpPost("register")]
        [Authorize(Roles = "admin")]
        public ActionResult register([FromBody] RegisterUser registerUser)
        {
            try
            {
                var user = _wcDbContext.Users.FirstOrDefault(x => x.Username == registerUser.Username);
                if (user != null)
                    return BadRequest(new { error = true, data = "Usuário já cadastrado!" });

                if (string.IsNullOrWhiteSpace(registerUser.Password) || string.IsNullOrWhiteSpace(registerUser.NomeCompleto) || string.IsNullOrWhiteSpace(registerUser.Email))
                    return BadRequest(new { error = true, data = "Verifique campos obrigatórios!" });

                var userStaging = new User() { Username = registerUser.Username, Role = registerUser.Role ?? "user", Password = registerUser.Password.ToHash(), Email = registerUser.Email, Document = registerUser.Document, NomeCompleto = registerUser.NomeCompleto, CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ) };
                var perfilConcessionaria = new UserProfile() { Permissao = "0000F", User = userStaging, CreatedAt = DateTime.Now.AddHours(SysConfig.TMZ) };
                _wcDbContext.Add(perfilConcessionaria);
                _wcDbContext.SaveChanges();

                user = _wcDbContext.Users.FirstOrDefault(x => x.Username == registerUser.Username);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshCredential refreshCred)
        {
            var token = tokenRefresher.Refresh(refreshCred);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

    }
}
