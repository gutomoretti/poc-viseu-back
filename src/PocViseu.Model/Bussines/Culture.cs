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
    [Table("TabCulture")]
    [Comment("Tabela de Log do Sistema")]
    public class Culture : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        [Comment("Identifiador")]
        public Int64 Id { get; set; }

        [Column("Descricao", TypeName = "varchar(500)")]
        [Comment("Descritivo")]
        public string Descricao { get; set; }

        [Column("Codigo", TypeName = "varchar(200)")]
        [Comment("Descritivo")]
        public string Codigo { get; set; }

        [Column("Indice", TypeName = "int")]
        [Comment("Indice de ordenacao")]
        public Int32? Indice { get; set; }
    }
}
