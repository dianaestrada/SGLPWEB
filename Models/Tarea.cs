using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGLPWEB.Models
{
    public class Tarea
    {
        public int TareaId { get; set; }

        [Required]

        public int CasoId { get; set; }
        public int ResponsableId { get; set; }
        public Usuario Responsable { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }

        public string Prioridad { get; set; } 

        public string Estado { get; set; } = "Pendiente";

        public string EstadoVencimiento
        {
            get
            {
                var hoy = DateTime.Today;
                if (FechaLimite < hoy && Estado != "Completado")
                    return "Vencida";
                else if (FechaLimite <= hoy.AddDays(2) && Estado != "Completado")
                    return "Por vencer";
                else if (Estado == "Completado")
                    return "Finalizada";
                else
                    return "Pendiente";
            }
        }

        [ForeignKey("CasoId")]
        public Caso Caso { get; set; }
    }
}
