namespace SistemaMantencion.Models
{
    public class RegistroMantencion
    {
        public int Id { get; set; }

        // Relación con Camioneta
        public int CamionetaId { get; set; }
        public Camioneta? Camioneta { get; set; }

        public string TipoMantencion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public int KilometrajeRegistrado { get; set; }
        public decimal? Costo { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;


    }
}