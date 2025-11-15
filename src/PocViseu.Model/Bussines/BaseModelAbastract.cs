using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PocViseu.Model.Auth;
using PocViseu.Model.Config;

namespace PocViseu.Model.Bussines
{
    public abstract class BaseModelAbastract
    {
        [ForeignKey("TabUsers")]
        [Column("IdUser", TypeName = "bigint")]
        [Comment("Identificadordo Usuario")]
        public Int64 UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [Column("CreatedAt", TypeName = "DateTime")]
        [Comment("Data de Criacao")]
        public DateTime CreatedAt { set; get; }

        [Column("UpdatedAt", TypeName = "DateTime")]
        [Comment("Data de Atualizacao")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now.AddHours(SysConfig.TMZ);

        [Column("HasDeleted", TypeName = "bit")]
        [Comment("Status Excluido 0- Nao | 1- SIM")]
        public bool Excluido { get; set; } = false;

       


    }
}
