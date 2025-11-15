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
            var userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value!);
            var traceKey = Guid.NewGuid().ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(register.pedido) || string.IsNullOrWhiteSpace(register.item))
                    return BadRequest(new { error = true, data = "Pedido e item são obrigatórios!" });

                var duplicated = _wcContext!.Process!
                    .FirstOrDefault(x => x.Excluido == false && x.pedido == register.pedido && x.item == register.item);
                if (duplicated != null)
                    return BadRequest(new { error = true, data = "Processo já cadastrado para o pedido/item informado!" });

                var now = DateTime.Now.AddHours(SysConfig.TMZ);
                var process = new Process()
                {
                    HashId = traceKey,
                    Status = (int)ProcessStatus.Waiting,
                    SchedulingExecType = (int)SchedulingExecTypeStatus.RunNow,
                    empresa = register.empresa,
                    nomeempresa = register.nomeempresa,
                    filial = register.filial,
                    nomefilial = register.nomefilial,
                    pedido = register.pedido,
                    item = register.item,
                    produto = register.produto,
                    descprod = register.descprod,
                    quantidade = register.quantidade,
                    lote = register.lote,
                    cultura = register.cultura,
                    cliente = register.cliente,
                    cnpjcpf = register.cnpjcpf,
                    chavenfe = register.chavenfe,
                    loja = register.loja,
                    razao = register.razao,
                    estado = register.estado,
                    municipio = register.municipio,
                    fazenda = register.fazenda,
                    inscricao = register.inscricao,
                    cep = register.cep,
                    ncm = register.ncm,
                    nota = register.nota,
                    serie = register.serie,
                    observacao = register.obsitem,
                    geolocal = register.geolocal,
                    descarte = register.descarte,
                    endcomple = register.endcomple,
                    mailagro = register.mailagro,
                    emaildepo = register.emaildepo,
                    emailAgronomo = register.emailAgronomo,
                    codUnidadeMedida = register.codUnidadeMedida,
                    qntEmbalagem = register.qntEmbalagem?.ToString(),
                    areaQntTratada = register.areaQntTratada?.ToString(),
                    Detailsjson = JsonConvert.SerializeObject(register),
                    UserId = userId,
                    CreatedAt = now,
                    UpdatedAt = now,
                };

                _wcContext.Add(process);
                _wcContext.SaveChanges();

                _ = _logSystemService.Log(JsonConvert.SerializeObject(register).ToLog("Processo importado", LogLevelStatus.Low, userId, traceKey));

                return Ok(new { idInsert = process.Id, process.HashId, Status = "Success" });
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
            var userId = long.Parse(claimsIdentity!.FindFirst(ClaimTypes.PrimarySid)?.Value!);
            try
            {
                Process? findItem = null;
                if (data.Id > 0)
                {
                    findItem = await _wcContext!.Process!.FirstOrDefaultAsync(x => x.Id == data.Id && x.Excluido == false);
                }

                if (findItem == null && !string.IsNullOrWhiteSpace(data.HashId))
                {
                    findItem = await _wcContext!.Process!.FirstOrDefaultAsync(x => x.HashId == data.HashId && x.Excluido == false);
                }

                if (findItem == null)
                    return BadRequest(new { error = true, data = "Processo não localizado!" });

                if (!string.IsNullOrWhiteSpace(data.StartedIn))
                    findItem.StartedIn = data.StartedIn.ParseDate(data.StartedInTime ?? "00:00");

                if (!string.IsNullOrWhiteSpace(data.FinishedAt))
                    findItem.FinishedAt = data.FinishedAt.ParseDate2();

                findItem.SchedulingExecType = data.SchedulingExecTypeId ?? findItem.SchedulingExecType;
                findItem.Status = data.Status ?? findItem.Status;
                findItem.MenssageResponse = data.MenssageResponse ?? findItem.MenssageResponse;

                findItem.filial = data.filial ?? findItem.filial;
                findItem.pedido = data.pedido ?? findItem.pedido;
                findItem.produto = data.produto ?? findItem.produto;
                findItem.descprod = data.descprod ?? findItem.descprod;
                if (data.quantidade.HasValue)
                    findItem.quantidade = data.quantidade.Value;
                findItem.lote = data.lote ?? findItem.lote;
                findItem.cultura = data.cultura ?? findItem.cultura;
                findItem.cliente = data.cliente ?? findItem.cliente;
                findItem.estado = data.estado ?? findItem.estado;
                findItem.municipio = data.municipio ?? findItem.municipio;
                findItem.fazenda = data.fazenda ?? findItem.fazenda;
                findItem.inscricao = data.inscricao ?? findItem.inscricao;
                findItem.cep = data.cep ?? findItem.cep;
                findItem.item = data.item ?? findItem.item;
                findItem.cpfAgronomo = data.cpfAgronomo ?? findItem.cpfAgronomo;
                findItem.nrArt = data.nrArt ?? findItem.nrArt;
                findItem.nrReceita = data.nrReceita ?? findItem.nrReceita;
                findItem.codCultura = data.codCultura ?? findItem.codCultura;
                findItem.codPraga = data.codPraga ?? findItem.codPraga;
                findItem.codTipoAplicacao = data.codTipoAplicacao ?? findItem.codTipoAplicacao;
                findItem.observacao = data.observacao ?? findItem.observacao;
                findItem.motivo = data.motivo ?? findItem.motivo;
                findItem.codUnidadeMedida = data.codUnidadeMedida ?? findItem.codUnidadeMedida;
                findItem.areaQntTratada = data.areaQntTratada ?? findItem.areaQntTratada;
                findItem.qntEmbalagem = data.qntEmbalagem ?? findItem.qntEmbalagem;
                findItem.Detailsjson = data.Detailsjson ?? findItem.Detailsjson;

                findItem.UpdatedAt = DateTime.Now.AddHours(SysConfig.TMZ);
                _wcContext.Update(findItem);
                await _wcContext.SaveChangesAsync();

                await _logSystemService.Log(JsonConvert.SerializeObject(data).ToLog("Processo atualizado", LogLevelStatus.Low, userId, findItem.HashId ?? string.Empty));

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
                var findItem = _wcContext!.Process!
                    .Include(x => x.ProcessAttachments.Where(p => p.Excluido == false))
                    .FirstOrDefault(x => x.Id == identification.Id);
                if (findItem == null)
                    return BadRequest(new { error = true, data = "Processo não localizado!" });

                var now = DateTime.Now.AddHours(SysConfig.TMZ);
                findItem.UpdatedAt = now;
                findItem.Excluido = true;

                if (findItem.ProcessAttachments.Any())
                {
                    foreach (var attachment in findItem.ProcessAttachments)
                    {
                        attachment.Excluido = true;
                        attachment.UpdatedAt = now;
                    }
                    _wcContext.ProcessAttachments!.UpdateRange(findItem.ProcessAttachments);
                }

                _wcContext.Update(findItem);
                _wcContext.SaveChanges();

                _ = _logSystemService.Log($"Processo {findItem.Id}".ToLog("Processo removido", LogLevelStatus.Low, findItem.UserId, findItem.HashId ?? string.Empty));

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
                view ??= new FilterProcessView();
                view.name ??= string.Empty;
                view.empresa ??= string.Empty;

                var query = _wcContext.Process!
                    .Where(x => x.Excluido == false)
                    .Include(x => x.ProcessAttachments.Where(a => a.Excluido == false))
                    .OrderByDescending(x => x.CreatedAt)
                    .AsQueryable();

                if (view.Id.HasValue)
                {
                    query = query.Where(x => x.Id == view.Id.Value);
                }

                if (!string.IsNullOrWhiteSpace(view.DateInit))
                {
                    var dateInit = view.DateInit.ParseDate2();
                    if (dateInit.HasValue)
                        query = query.Where(x => x.CreatedAt >= dateInit.Value);
                }

                if (!string.IsNullOrWhiteSpace(view.DateEnd))
                {
                    var dateEnd = view.DateEnd.ParseDate2End();
                    if (dateEnd.HasValue)
                        query = query.Where(x => x.CreatedAt <= dateEnd.Value);
                }

                var result = query
                    .Take(1000)
                    .ToList()
                    .Where(p => ProcessExt.Predicate(view).Invoke(p));

                return Ok(result);
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
                    var HashIdContext = $"{new Random().Next()}-";
                    try
                    {
                        string _code = "";

                        file.fileName = HashIdContext + file.fileName;

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.fileName);

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
