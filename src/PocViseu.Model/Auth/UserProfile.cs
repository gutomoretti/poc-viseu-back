using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PocViseu.Model.Bussines;

namespace PocViseu.Model.Auth
{
    [Table("TabUsersProfile")]
    [Comment("Tabela de Controle de Acesso de cada Perfil de Usuario")]
    public class UserProfile : BaseModelAbastract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdUserProfile", TypeName = "bigint")]
        [Comment("Identificador do Perfil")]
        public Int64 Id { get; set; }


        [Column("ProfileRule", TypeName = "varchar(50)")]
        [Comment("Mapa de Bits para controle de acesso aos menus")]
        public string Permissao { get; set; }

    }
}
