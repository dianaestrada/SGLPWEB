using SGLPWEB.Models.ViewModels;

namespace SGLPWEB.Models.ViewModels
{
    public class DashboardClienteViewModel
    {
        public int TotalCasos { get; set; }
        public int CasosActivos { get; set; }
        public int CasosFinalizados { get; set; }

        public int TareasPendientes { get; set; }
        public int PlazosProximos { get; set; }

        public Evidencia? UltimaEvidencia { get; set; }
        public Caso? UltimoCaso { get; set; }
        public List<Evidencia> Evidencias { get; set; }
    }
}