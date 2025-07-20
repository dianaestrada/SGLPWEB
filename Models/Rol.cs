using System.ComponentModel.DataAnnotations;

namespace SGLPWEB.Models
{
    public class Rol
    {
        public int RolId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}