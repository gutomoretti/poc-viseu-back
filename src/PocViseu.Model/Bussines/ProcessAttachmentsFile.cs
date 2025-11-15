using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocViseu.Model.Bussines
{
    [Table("TabProcessAttachmentsFile")]
    [Comment("Tabela de Anexo Arquivos do Processo")]
    public class ProcessAttachmentsFile : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [Column("NO_ARQUIVO", TypeName = "varchar(150)")]
        [Comment("Nome do Arquivo Anexo")]
        public string? NomeArquivo { get; set; }

        [Column("IM_ARQUIVO", TypeName = "text")]
        [Comment("Arquivo Anexo em Formato Binario")]
        public string? Arquivo { get; set; }


    }
}
