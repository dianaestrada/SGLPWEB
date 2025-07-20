using System;
using System.ComponentModel.DataAnnotations;

namespace SGLPWEB.Models.DTO
{
    public class PlazoDTO
    {
        public int PlazoId { get; set; }
        [Required]
        public int CasoId { get; set; }

        [Required]
        public DateTime FechaEvento { get; set; }

        public string TipoEvento { get; set; }

        public string Descripcion { get; set; }
    }
}
