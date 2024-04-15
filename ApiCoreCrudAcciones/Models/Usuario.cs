using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiCoreCrudAcciones.Models
{

    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("SALT")]
        public string Salt { get; set; }

        //[Column("IMAGEN")]
        //public string Imagen { get; set; }

        [Column("PASS")]
        public byte[] Password { get; set; }

        //[Column("ACTIVO")]
        //public bool Activo { get; set; }

        [Column("TOKENMAIL")]
        public string TokenMail { get; set; }
    }
}
