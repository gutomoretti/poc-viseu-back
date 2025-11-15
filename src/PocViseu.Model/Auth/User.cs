using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PocViseu.Model.Config;

namespace PocViseu.Model.Auth
{
    [Table("TabUsers")]
    [Index(nameof(Username), IsUnique = true)]
    [Comment("Tabela de Controle de acessos ao Sistema")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        [Comment("Identificador do Usuario")]
        public Int64 Id { get; set; }

        [Required]
        [Column("Name", TypeName = "varchar(50)")]
        [Comment("Nome do Usuario")]
        public string? Username { get; set; }

        [Required]
        [Column("Email", TypeName = "varchar(100)")]
        [Comment("E-mail do Usuario para login ao Sigicor")]
        public string? Email { get; set; }

        [Required]
        [JsonIgnore]
        [Column("Password", TypeName = "varchar(100)")]
        [Comment("Senha do Usuarios formato HashCode")]
        public string? Password { get; set; }

        [Required]
        [Column("Rule", TypeName = "varchar(30)")]
        [Comment("Perfil da Regra Usuario, Admin, etc...")]
        public string? Role { get; set; }

        [MaxLength(100)]
        [Column("FullName", TypeName = "varchar(100)")]
        [Comment("Nome Completo Usuario")]
        public string? NomeCompleto { get; set; }

        [Column("Document", TypeName = "varchar(100)")]
        [Comment("Documento Usuario")]
        public string? Document { get; set; }

        [Column("CreatedAt", TypeName = "DateTime")]
        [Comment("Data de Criacao")]
        public DateTime CreatedAt { set; get; }

        [Column("UpdatedAt", TypeName = "DateTime")]
        [Comment("Data de Atualizacao")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now.AddHours(SysConfig.TMZ);

        [Column("HasDeleted", TypeName = "bit")]
        [Comment("Status Excluido")]
        public bool Excluido { get; set; } = false;

        public virtual List<UserProfile> Perfil { get; set; } = new List<UserProfile>();

    }
}

