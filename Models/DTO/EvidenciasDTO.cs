using System;
using System.ComponentModel.DataAnnotations;

namespace SGLPWEB.Models.DTO
{
    public class EvidenciaDTO
    {
        public int EvidenciaId { get; set; }
        [Required]
        public int TareaId { get; set; }

        public string NombreArchivo { get; set; }

        public string RutaArchivo { get; set; }

        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}
