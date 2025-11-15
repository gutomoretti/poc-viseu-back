using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PocViseu.Model.Bussines;

namespace PocViseu.Model.Config
{
    [Table("TabWebConfig")]
    [Comment("Tabela de Configuracao do Sistema")]
    public class WebcorpConfig : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [Column("ParamKey", TypeName = "varchar(500)")]
        [Comment("Nome/Key da Configuracao")]
        public string? ParamKey { set; get; }

        [Column("ParamValue", TypeName = "varchar(500)")]
        [Comment("Valor da Configuracao")]
        public string? ParamValue { set; get; }

        [Column("ParamDescription", TypeName = "varchar(500)")]
        [Comment("Descricao do item")]
        public string? ParamDesc { set; get; }
    }
}
