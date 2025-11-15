using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Auth;
using PocViseu.Model.Bussines;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;
using PocViseu.Model.ModelView.User;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace PocViseu.Api.Services
{
    public class HttpBot : IHttpBot
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogSystemService _logSystemService;
        private readonly IServiceScopeFactory _scopeFactory;

        private HttpClient _httpClientInternal = null;
        private string code = string.Empty;
        private string _tracekey = string.Empty;
        private string _internalParam = string.Empty;
        private string _internalParam2 = string.Empty;
        private string _pathBot = string.Empty;
        private string url = string.Empty;
        private string _endpoint = string.Empty;

        private string _restUser = string.Empty;
        private string _restPass = string.Empty;

        private string _defaultMessage = @"
        Do not reply to this email, automatic sending.

        Error connecting to the robot to run the automation. Please contact technical support.

        Greetings,

        Bot Control.
        ";

        public HttpBot(HttpClient httpClient, IConfiguration configuration, ILogSystemService logSystemService,
              IServiceScopeFactory scopeFactory)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logSystemService = logSystemService;
            _scopeFactory = scopeFactory;
        }

        private async Task LoadEnv()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var wcContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();

                //PATH_BOT
                var queryParamBotPath = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "PATH_BOT")
                                  .OrderBy(x => x.Id)
                                 .ToList()
                                        select p;
                var itemParamBotParam = queryParamBotPath.FirstOrDefault();
                if (itemParamBotParam != null)
                {
                    this._pathBot = itemParamBotParam.ParamValue ?? "0";
                }

                //URL_BOT_ENDPOINT
                var queryParamBotEndpoint = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "URL_BOT_ENDPOINT")
                                  .OrderBy(x => x.Id)
                                 .ToList()
                                            select p;
                var itemParamBotEndpoint = queryParamBotEndpoint.FirstOrDefault();
                if (itemParamBotEndpoint != null)
                {
                    this._endpoint = itemParamBotEndpoint.ParamValue ?? "0";
                }

                //PARAM_DEBUG_BOT
                var queryParam = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "PARAM_DEBUG_BOT")
                                   .OrderBy(x => x.Id)
                                  .ToList()
                                 select p;
                var itemParam = queryParam.FirstOrDefault();
                if (itemParam != null)
                {
                    this._internalParam = itemParam.ParamValue ?? "0";
                }


                //CODE_BOT
                var queryCode = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "CODE_BOT")
                                   .OrderBy(x => x.Id)
                                  .ToList()
                                select p;
                var itemCode = queryCode.FirstOrDefault();
                this.code = "0";
                if (itemCode != null)
                {
                    this.code = itemCode.ParamValue ?? "0";
                }

                //RES USER
                var queryRestUser = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "REST_USER")
                                   .OrderBy(x => x.Id)
                                  .ToList()
                                 select p;
                var itemRestUser = queryRestUser.FirstOrDefault();
                if (itemRestUser != null)
                {
                    this._restUser = itemRestUser.ParamValue ?? "0";
                }

                //RES PASS
                var queryRestPass = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "REST_PASSWORD")
                                   .OrderBy(x => x.Id)
                                  .ToList()
                                    select p;
                var itemRestPass = queryRestPass.FirstOrDefault();
                if (itemRestPass != null)
                {
                    this._restPass = itemRestPass.ParamValue ?? "0";
                }


                //URL_BOT
                var query = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "URL_BOT")
                                   .OrderBy(x => x.Id)
                                  .ToList()
                            select p;

                var item = query.FirstOrDefault();
                var _url = string.Empty;
                if (item != null && !string.IsNullOrEmpty(item.ParamValue))
                {
                    _url = item.ParamValue;
                }
                else
                {
                    _url = _configuration["UrlBot"];
                }
                this.url = _url;

                if (_httpClientInternal != null)
                    _httpClientInternal.Dispose();

                _httpClientInternal = new HttpClient();
                _httpClientInternal.BaseAddress = new Uri(_url);

            }
        }

        public async Task<bool> ExecuteSome(Model.Bussines.Process process, bool execAll, int schedulingPlanType = 0)
        {
            try
            {
                _tracekey = process.HashId!;
                _internalParam = process.MenssageResponse!;
                _internalParam2 = process.MenssageResponseP2!;

                await LoadEnv();

             
                return true;
            }
            catch (Exception Ex)
            {
                _ = _logSystemService.Log(Ex.Message.ToLog("System Alert", LogLevelStatus.High, 1, _tracekey));
                return false;
            }
        }



    }
}
