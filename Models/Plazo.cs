using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGLPWEB.Models
{
    public class Plazo
    {
        public int PlazoId { get; set; }

        [Required]
        public int CasoId { get; set; }
        [Required]
        public DateTime FechaEvento { get; set; }

        public string TipoEvento { get; set; }

        public string Descripcion { get; set; }

        [JsonIgnore]
        public Caso Caso { get; set; }
        public string EstadoPlazo
        {
            get
            {
                var hoy = DateTime.Today;
                if (FechaEvento < hoy)
                    return "Vencido";
                else if (FechaEvento <= hoy.AddDays(3))
                    return "Próximo";
                else
                    return "Programado";
            }
        }
    }
}
