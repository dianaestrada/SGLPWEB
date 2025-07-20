using System;
using System.ComponentModel.DataAnnotations;

namespace SGLPWEB.Models.DTO
{
    public class CasoDTO
    {
        public int CasoId { get; set; }
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public string TipoCaso { get; set; }

        public DateTime FechaInicio { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; } = "Abierto";
    }
}
