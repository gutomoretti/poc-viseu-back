using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PocViseu.Model.Bussines
{
    [Table("TabProcessAttachments")]
    [Comment("Tabela de Anexo do Processo")]
    public class ProcessAttachments : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [ForeignKey("TabProcess")]
        [Column("ProcessId", TypeName = "bigint")]
        [Comment("Identificacao do Processo")]
        public Int64 ProcessId { get; set; }

        [JsonIgnore]
        public virtual Process Process { get; set; }

        [ForeignKey("TabProcessAttachmentsFile")]
        [Column("ProcessAttachmentsFileId", TypeName = "bigint")]
        [Comment("Identificacao do Processo")]
        public Int64? ProcessAttachmentsFileId { get; set; }

        [JsonIgnore]
        public virtual ProcessAttachmentsFile ProcessAttachmentsFile { get; set; }

        [Column("NO_ARQUIVO", TypeName = "varchar(150)")]
        [Comment("Nome do Arquivo Anexo")]
        public string? NomeArquivo { get; set; }

        [Column("Subitem", TypeName = "int")]
        [Comment("Identificacao do subitem Processo")]
        public int? Subitem { get; set; }


    }
}
