using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGLPWEB.Models
{
    public class Evidencia
    {
        public int EvidenciaId { get; set; }

        [Required]
        public int TareaId { get; set; }

        public int CasoId { get; set; }
        public Caso Caso { get; set; }

        public string NombreArchivo { get; set; }

        public string RutaArchivo { get; set; }

        public DateTime FechaSubida { get; set; } = DateTime.Now;

        [ForeignKey("TareaId")]
        public Tarea Tarea { get; set; }
    }
}
