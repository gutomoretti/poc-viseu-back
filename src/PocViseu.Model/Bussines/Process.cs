using PocViseu.Model.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocViseu.Model.Bussines
{

    [Table("TabProcess")]
    [Index(nameof(HashId), IsUnique = true)]
    [Comment("Tabela de Processo do Sistema")]
    public class Process : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [Column("HashId", TypeName = "varchar(100)")]
        [Comment("HashId do Processamento")]
        public string? HashId { set; get; }


        [Column("StartedIn", TypeName = "DateTime")]
        [Comment("Data de Inicio do Processo")]
        public DateTime? StartedIn { set; get; }

        [Column("FinishedAt", TypeName = "DateTime")]
        [Comment("Data de Finalizacao")]
        public DateTime? FinishedAt { set; get; }

        [Column("SchedulingExecType", TypeName = "int")]
        [Comment("Tipo de Execucao do Agendamento  0-Executar Agora; 1-Agendado;")]
        public Int32? SchedulingExecType { get; set; } = 0;

        [Column("Status", TypeName = "int")]
        [Comment("Status do Processo 10 - QUEUE; 11-STARTED; 12-DONE; 13-ERROR; ||  0-Aguardando; 1-Iniciado; 2-Concluido; 3-Falha  ")]
        public Int32? Status { get; set; } = 10;

        [Column("MenssageResponse", TypeName = "varchar(1000)")]
        [Comment("Resposta de Processamento")]
        public string? MenssageResponse { set; get; }

        [Column("MenssageResponse2", TypeName = "varchar(1000)")]
        [Comment("Resposta de Processamento parametro 2")]
        public string? MenssageResponseP2 { set; get; }

        //campos WS-PROTHEUS
        [Column("empresa", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? empresa { set; get; }

        [Column("nomeempresa", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? nomeempresa { set; get; }

        [Column("filial", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? filial { set; get; }

        [Column("nomefilial", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? nomefilial { set; get; }

        [Column("pedido", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? pedido { set; get; }

        [Column("produto", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? produto { set; get; }

        [Column("descprod", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? descprod { set; get; }


        [Column("quantidade", TypeName = "decimal(10, 2)")]
        [Comment("Campos Protheus")]
        public Decimal? quantidade { set; get; }

        [Column("lote", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? lote { set; get; }


        [Column("cultura", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? cultura { set; get; }

        [Column("cliente", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? cliente { set; get; }

        [Column("cnpjcpf", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? cnpjcpf { set; get; }

        [Column("chavenfe", TypeName = "varchar(300)")]
        [Comment("Campos Protheus")]
        public string? chavenfe { set; get; }

        [Column("estado", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? estado { set; get; }

        [Column("municipio", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? municipio { set; get; }

        [Column("fazenda", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? fazenda { set; get; }

        [Column("inscricao", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? inscricao { set; get; }

        [Column("cep", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? cep { set; get; }

        [Column("item", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? item { set; get; }

        //
        [Column("cpfAgronomo", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? cpfAgronomo { set; get; }
        //
        [Column("nrArt", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? nrArt { set; get; }
        // 
        [Column("nrReceita", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? nrReceita { set; get; }
        //
        [Column("codCultura", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? codCultura { set; get; }
        //
        [Column("codPraga", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? codPraga { set; get; }
        //
        [Column("codTipoAplicacao", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? codTipoAplicacao { set; get; }

        [Column("emailAgronomo", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? emailAgronomo { set; get; }

        [Column("loja", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? loja { set; get; }

        [Column("razao", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? razao { set; get; }

        [Column("ncm", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? ncm { set; get; }

        [Column("nota", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? nota { set; get; }

        [Column("serie", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? serie { set; get; }

        [Column("codUnidadeMedida", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? codUnidadeMedida { set; get; }

        [Column("areaQntTratada", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? areaQntTratada { set; get; }

        [Column("qntEmbalagem", TypeName = "varchar(100)")]
        [Comment("Campos Protheus")]
        public string? qntEmbalagem { set; get; }

        [Column("geolocal", TypeName = "varchar(200)")]
        [Comment("Campos Protheus")]
        public string? geolocal { set; get; }

        [Column("descarte", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? descarte { set; get; }

        [Column("endcomple", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? endcomple { set; get; }

        [Column("mailagro", TypeName = "varchar(150)")]
        [Comment("Campos Protheus")]
        public string? mailagro { set; get; }

        [Column("emaildepo", TypeName = "varchar(150)")]
        [Comment("Campos Protheus")]
        public string? emaildepo { set; get; }

        //--end-campos WS-PROTHEUS

        [Column("observacao", TypeName = "varchar(500)")]
        [Comment("Campos Protheus")]
        public string? observacao { set; get; }

        [Column("motivo", TypeName = "varchar(500)")]
        [Comment("Motivo Rejeicao")]
        public string? motivo { set; get; }

        [ForeignKey("TabUsers")]
        [Column("IdUserContact", TypeName = "bigint")]
        [Comment("Identificadordo Usuario Responsavel Tecnico")]
        public Int64? IdUserContactId { get; set; }
        public virtual User? IdUserContact { get; set; }

        [Column("detailsjs", TypeName = "text")]
        [Comment("Detalhes Json")]
        public string? Detailsjson { set; get; }

        public virtual List<ProcessAttachments> ProcessAttachments { get; set; } = new List<ProcessAttachments>();


        [NotMapped]
        public virtual string? codCulturaResolve { get; set; }

        [NotMapped]
        public virtual string? codPragaResolve { get; set; }

        [NotMapped]
        public virtual string? codTipoAplicacaoResolve { get; set; }

    }

    public enum ProcessStatus
    {
        /// <summary>
        /// Aguardando
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// Iniciado
        /// </summary>
        Started = 1,
        /// <summary>
        /// Concluido
        /// </summary>
        Done = 2,
        /// <summary>
        /// Falha
        /// </summary>
        Error = 3,
        /// <summary>
        /// Status QUEUE NEW
        /// </summary>
        Queue = 10,
        Queue_Started = 11,
        Queue_Done = 12,
        Queue_Error = 13,
        Queue_TimeOut = 14
    }

    public enum SchedulingExecTypeStatus
    {
        /// <summary>
        /// Executar Agora
        /// </summary>
        RunNow = 0,
        /// <summary>
        /// Agendado
        /// </summary>
        Scheduled = 1,
        Queue_RunNow = 11,
        Queue_RunNowAll = 12
    }
}
