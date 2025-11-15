using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocViseu.Model.ModelView
{
    public class ProcessModelView
    {
        public Int64 Id { get; set; }
        public string? HashId { set; get; }
        public Int64 ProjectId { get; set; }
        public String? StartedIn { set; get; }
        public String? StartedInTime { set; get; }
        public String? FinishedAt { set; get; }
        public string? SchedulingExecType { get; set; }
        public Int32? SchedulingExecTypeId { get; set; }
        public Int32? Status { get; set; }
        public string? MenssageResponse { set; get; }

        //campos WS-PROTHEUS

        public string? filial { set; get; }
        public string? pedido { set; get; }
        public string? produto { set; get; }
        public string? descprod { set; get; }
        public Int32? quantidade { set; get; }
        public string? lote { set; get; }
        public string? cultura { set; get; }
        public string? cliente { set; get; }
        public string? estado { set; get; }
        public string? municipio { set; get; }
        public string? fazenda { set; get; }
        public string? inscricao { set; get; }
        public string? cep { set; get; }
        public string? item { set; get; }

        public string? cpfAgronomo { set; get; }
        public string? nrArt { set; get; }
        public string? nrReceita { set; get; }
        public string? codCultura { set; get; }
        public string? codPraga { set; get; }
        public string? codTipoAplicacao { set; get; }

        //--end-campos WS-PROTHEUS
        public string? observacao { set; get; }
        public string? motivo { set; get; }
        public string? codUnidadeMedida { set; get; }
        public string? areaQntTratada { set; get; }
        public string? qntEmbalagem { set; get; }

        public string? Detailsjson { set; get; }
    }

    public class ProcessManualModelView
    {
        public string? parametro { set; get; }
        public string? parametro2 { set; get; }
        public string? detailsJson { set; get; }
        public bool? execAllProjects { get; set; } = false;
        public long? userId { get; set; }
        public string? userMail { get; set; }
    }


    public class SubItemProcessModelView
    {
        public Int64 Id { get; set; }
        public long culturaId { get; set; }
        public long pragaId { get; set; }
        public long tipoAplicacaoId { get; set; }
        public string? areaQntTratada { set; get; }
        public string? nrArt { set; get; }
        public string? nrReceita { set; get; }
        public string? codCultura { set; get; }
        public string? codPraga { set; get; }
        public string? codTipoAplicacao { set; get; }
    }

}
