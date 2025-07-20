using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGLPWEB.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string UsuarioPortal { get; set; }

        public string ClaveHash { get; set; }

        [JsonIgnore]
        public ICollection<Caso> Casos { get; set; } = new List<Caso>();


    }
}
