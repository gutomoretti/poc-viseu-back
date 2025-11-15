using PocViseu.Api.Middleware;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Infrastructure.Querys;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Process = PocViseu.Model.Bussines.Process;

namespace PocViseu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ProcessController : ControllerBase
    {
        private readonly WebControlDbContext _wcContext;
        private readonly IHttpBot _httpBot;
        private readonly ILogSystemService _logSystemService;
        private readonly IMailService _mailService;

        public ProcessController(WebControlDbContext wcContext, IHttpBot httpBot,
            ILogSystemService logSystemService, IMailService mailService)
        {
            this._wcContext = wcContext;
            this._httpBot = httpBot;
            this._logSystemService = logSystemService;
            this._mailService = mailService;
        }


        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult create([FromBody] ProcessImportModel register)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;            
            int idInsert = 0;
            try
            {                

                return Ok(new { idInsert, Status = "Success" });
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> update([FromBody] ProcessModelView data)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
               
                return Ok();
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
                // Alterar para Excluido true
                //findItem!.Excluido = true;

                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpGet("list")]
        public ActionResult List([FromQuery] FilterProcessView? view)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value!);
            try
            {
                //listar somente findItem!.Excluido = false;
                return Ok();             
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }


        [HttpPost("upload")]
        [RequestSizeLimit(15_000_000)] //5MB
        public async Task<string> upload([FromForm] FileModel file)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value;
            try
            {
                if (file.formFile.Length > 0)
                {
                    var HashIdContext = $"{new Random().Next()}-" ;
                    try
                    {
                        string _code = "";

                        file.fileName = HashIdContext + file.fileName;

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",  file.fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            file.formFile.CopyTo(stream);
                        }

                        using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                        {
                                                      
                            var anexo = new ProcessAttachments()
                            {
                                ProcessId = file.ProcessId!.Value,
                                NomeArquivo = file.fileName,
                                Subitem = file.Subitem!.Value,
                                CreatedAt = DateTime.Now.AddHours(-3),
                                UserId = int.Parse(userId!),
                            };
                            _wcContext!.Add(anexo);                           

                            _wcContext.SaveChanges();

                            _code = anexo.Id.ToString();

                            var findItem = _wcContext.Process!.Single(x => x.Excluido == false && x.Id == file.ProcessId.Value);

                        }

                        return await Task.FromResult($"{_code}");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                return await Task.FromResult("FAIL");
            }
            catch (Exception e)
            {
                return await Task.FromResult("FAIL: " + e.Message);
            }
        }

        [HttpPut("deleteattachments")]
        public ActionResult deleteattachments([FromBody] IdentificationView identification)
        {
            try
            {
                var findItem = _wcContext!.ProcessAttachments!.Single(x => x.Id == identification.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "nao localizado!" });

                findItem!.UpdatedAt = DateTime.Now.AddHours(SysConfig.TMZ);
                findItem!.Excluido = true;

                _wcContext.Update(findItem);

                _wcContext.SaveChanges();

                var findItemP = _wcContext.Process!.Single(x => x.Excluido == false && x.Id == findItem.ProcessId);

                _mailService.Send($"Removido Anexo no Pedido {findItemP.pedido} arquivo {findItem.NomeArquivo}", $"Removido Anexo no Pedido {findItemP.pedido}", findItemP.mailagro, findItemP!);


                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [HttpGet("download")]
        public async Task<FileStreamResult> download(long id)
        {

            var findItem = _wcContext!.ProcessAttachments!.FirstOrDefault(x => x.Id == id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", findItem?.NomeArquivo!);
            var stream = new FileStream(path, FileMode.Open);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = findItem?.NomeArquivo,
            };

        }

    }
}
