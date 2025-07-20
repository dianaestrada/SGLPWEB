using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGLPWEB.Models
{
    public class Caso
    {
        public int CasoId { get; set; }
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public string TipoCaso { get; set; }

        public DateTime FechaInicio { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; } = "Abierto";

        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
        public ICollection<Plazo> Plazos { get; set; } = new List<Plazo>();
        public ICollection<Evidencia> Evidencias { get; set; } = new List<Evidencia>();

        [BindNever]
        [JsonIgnore]
        [Display(Name = "ClienteId")]
        public Cliente Cliente { get; set; }
    }
}