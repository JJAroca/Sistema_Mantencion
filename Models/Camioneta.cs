using System.ComponentModel.DataAnnotations;

namespace SistemaMantencion.Models
{
    public class Camioneta
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Patente { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;
        
        public int Anio { get; set; }
        
        public int Kilometraje { get; set; }
        
        [Required]
        public string Estado { get; set; } = "Disponible"; // Disponible, EnArriendo, EnMantencion
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        public DateTime? FechaUltimaMantencion { get; set; }
        
        // Soft Delete
        public bool Activo { get; set; } = true;
        public DateTime? FechaEliminacion { get; set; }
        
        // Relación con Mantenciones
        public ICollection<RegistroMantencion> Mantenciones { get; set; } = new List<RegistroMantencion>();
    }

    public class RegistroMantencion
    {
        public int Id { get; set; }
        
        public int CamionetaId { get; set; }
        public Camioneta Camioneta { get; set; } = null!;
        
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        
        public DateTime? FechaFin { get; set; }
        
        [Required]
        public string TipoMantencion { get; set; } = string.Empty; // Preventiva, Correctiva, Revision
        
        public string? Descripcion { get; set; }
        
        public int KilometrajeRegistrado { get; set; }
        
        public decimal? Costo { get; set; }
        
        public string Estado { get; set; } = "EnProceso"; // EnProceso, Completada
        
        public string? Observaciones { get; set; }
    }

    public class HistorialCamioneta
    {
        public int Id { get; set; }
        
        public int CamionetaId { get; set; }
        public Camioneta Camioneta { get; set; } = null!;
        
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        [Required]
        public string Accion { get; set; } = string.Empty; // Retiro, Reintegro, Mantencion, Actualizacion
        
        public string? Motivo { get; set; }
        
        public int? KilometrajeAnterior { get; set; }
        
        public int? KilometrajeNuevo { get; set; }
        
        public string? EstadoAnterior { get; set; }
        
        public string? EstadoNuevo { get; set; }
    }
}