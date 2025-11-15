using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocViseu.Model.Bussines
{

    [Table("TabLogSystem")]
    [Comment("Tabela de Log do Sistema")]
    public class LogSystem : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [Column("Description", TypeName = "text")]
        [Comment("Descricao")]
        public string? Description { set; get; }

        [Column("Value", TypeName = "text")]
        [Comment("Valor")]
        public string? Value { set; get; }

        [Column("Level", TypeName = "int")]
        [Comment("Nivel de criticidade  0-baixa; 1-media; 3-alta")]
        public Int32? Level { get; set; } = 0;

        [Column("TraceKey", TypeName = "text")]
        [Comment("TraceKey codigo de rastreamento do log")]
        public string? TraceKey { set; get; }
    }

    public enum LogLevelStatus
    {
        /// <summary>
        /// Baixa
        /// </summary>
        Low = 0,
        /// <summary>
        /// Media
        /// </summary>
        Medium = 1,
        /// <summary>
        /// Alta
        /// </summary>
        High = 2,
    }
}
