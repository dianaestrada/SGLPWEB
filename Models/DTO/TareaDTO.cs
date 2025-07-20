using System;
using System.ComponentModel.DataAnnotations;

namespace SGLPWEB.Models.DTO
{
    public class TareaDTO
    {
        public int TareaId { get; set; }
        [Required]
        public int CasoId { get; set; }

        public int ResponsableId { get; set; } 

        public string Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; }

        public string Estado { get; set; } = "Pendiente";
    }
}
