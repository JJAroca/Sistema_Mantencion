using Grpc.Core;
using ArriendoGrpc;
using SistemaMantencion.Data;
using SistemaMantencion.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaMantencion.Services
{
    public class MantencionServiceImpl : ArriendoGrpc.MantencionService.MantencionServiceBase
    {
        private readonly MantencionDbContext _context;
        private readonly ILogger<MantencionServiceImpl> _logger;

        public MantencionServiceImpl(MantencionDbContext context, ILogger<MantencionServiceImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<CamionetasResponse> ObtenerCamionetasDisponibles(Empty request, ServerCallContext context)
        {
            var camionetas = await _context.Camionetas
                .Where(c => c.Estado == "Disponible")
                .ToListAsync();

            var response = new CamionetasResponse();
            
            foreach (var camioneta in camionetas)
            {
                response.Camionetas.Add(new CamionetaInfo
                {
                    Id = camioneta.Id,
                    Patente = camioneta.Patente,
                    Marca = camioneta.Marca,
                    Modelo = camioneta.Modelo,
                    Kilometraje = camioneta.Kilometraje,
                    Estado = camioneta.Estado
                });
            }

            _logger.LogInformation($"Devolviendo {camionetas.Count} camionetas disponibles");
            return response;
        }

        public override async Task<CamionetaResponse> ObtenerCamioneta(CamionetaRequest request, ServerCallContext context)
        {
            try
            {
                var camioneta = await _context.Camionetas.FindAsync(request.Id);
                
                if (camioneta == null)
                {
                    return new CamionetaResponse
                    {
                        Success = false,
                        Message = "Camioneta no encontrada"
                    };
                }

                return new CamionetaResponse
                {
                    Success = true,
                    Message = "Camioneta encontrada",
                    Camioneta = new CamionetaInfo
                    {
                        Id = camioneta.Id,
                        Patente = camioneta.Patente,
                        Marca = camioneta.Marca,
                        Modelo = camioneta.Modelo,
                        Kilometraje = camioneta.Kilometraje,
                        Estado = camioneta.Estado
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener camioneta: {ex.Message}");
                return new CamionetaResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public override async Task<RetiroResponse> SolicitarRetiroCamioneta(RetiroRequest request, ServerCallContext context)
        {
            try
            {
                var camioneta = await _context.Camionetas.FindAsync(request.CamionetaId);
                
                if (camioneta == null)
                {
                    return new RetiroResponse
                    {
                        Success = false,
                        Message = "Camioneta no encontrada"
                    };
                }

                if (camioneta.Estado != "Disponible")
                {
                    return new RetiroResponse
                    {
                        Success = false,
                        Message = $"Camioneta no disponible. Estado actual: {camioneta.Estado}"
                    };
                }

                // Registrar historial
                var historial = new HistorialCamioneta
                {
                    CamionetaId = camioneta.Id,
                    Accion = "Retiro",
                    Motivo = request.Motivo,
                    EstadoAnterior = camioneta.Estado,
                    EstadoNuevo = "EnArriendo",
                    KilometrajeAnterior = camioneta.Kilometraje,
                    Fecha = DateTime.Now
                };

                _context.Historial.Add(historial);
                
                // Actualizar estado
                camioneta.Estado = "EnArriendo";
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Camioneta {camioneta.Patente} retirada para arriendo");

                return new RetiroResponse
                {
                    Success = true,
                    Message = "Camioneta retirada exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al retirar camioneta: {ex.Message}");
                return new RetiroResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public override async Task<ReintegroResponse> ReintegrarCamioneta(ReintegroRequest request, ServerCallContext context)
        {
            try
            {
                var camioneta = await _context.Camionetas.FindAsync(request.CamionetaId);
                
                if (camioneta == null)
                {
                    return new ReintegroResponse
                    {
                        Success = false,
                        Message = "Camioneta no encontrada"
                    };
                }

                // Registrar historial
                var historial = new HistorialCamioneta
                {
                    CamionetaId = camioneta.Id,
                    Accion = "Reintegro",
                    Motivo = "Fin de arriendo",
                    EstadoAnterior = camioneta.Estado,
                    EstadoNuevo = "Disponible",
                    KilometrajeAnterior = camioneta.Kilometraje,
                    KilometrajeNuevo = request.KilometrajeActual,
                    Fecha = DateTime.Now
                };

                _context.Historial.Add(historial);

                // Actualizar estado y kilometraje
                camioneta.Estado = "Disponible";
                camioneta.Kilometraje = request.KilometrajeActual;
                
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Camioneta {camioneta.Patente} reintegrada con {request.KilometrajeActual} km");

                return new ReintegroResponse
                {
                    Success = true,
                    Message = "Camioneta reintegrada exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al reintegrar camioneta: {ex.Message}");
                return new ReintegroResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public override async Task<KilometrajeResponse> ActualizarKilometraje(KilometrajeRequest request, ServerCallContext context)
        {
            try
            {
                var camioneta = await _context.Camionetas.FindAsync(request.CamionetaId);
                
                if (camioneta == null)
                {
                    return new KilometrajeResponse
                    {
                        Success = false,
                        Message = "Camioneta no encontrada"
                    };
                }

                // Registrar historial
                var historial = new HistorialCamioneta
                {
                    CamionetaId = camioneta.Id,
                    Accion = "Actualizacion",
                    Motivo = "Actualización de kilometraje",
                    KilometrajeAnterior = camioneta.Kilometraje,
                    KilometrajeNuevo = request.Kilometraje,
                    Fecha = DateTime.Now
                };

                _context.Historial.Add(historial);
                
                camioneta.Kilometraje = request.Kilometraje;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Kilometraje actualizado para {camioneta.Patente}: {request.Kilometraje} km");

                return new KilometrajeResponse
                {
                    Success = true,
                    Message = "Kilometraje actualizado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar kilometraje: {ex.Message}");
                return new KilometrajeResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}