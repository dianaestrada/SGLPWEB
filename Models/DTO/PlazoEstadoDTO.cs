using System;
using SGLPWEB.Models;

namespace SGLPWEB.Models.DTO
{
    public class PlazoEstadoDTO
    {
        public int PlazoId { get; set; }
        public int CasoId { get; set; }
        public DateTime FechaEvento { get; set; }
        public string TipoEvento { get; set; }
        public string Descripcion { get; set; }
        public string EstadoPlazo { get; set; }  
    }
}