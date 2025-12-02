using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaMantencion.Data;
using SistemaMantencion.Models;

namespace SistemaMantencion.Controllers
{
    public class CamionetasController : Controller
    {
        private readonly MantencionDbContext _context;

        public CamionetasController(MantencionDbContext context)
        {
            _context = context;
        }

        // GET: Camionetas
        public async Task<IActionResult> Index(string? estadoFiltro, bool mostrarEliminadas = false)
        {
            var query = _context.Camionetas.AsQueryable();

            // Filtrar por activo/inactivo
            if (!mostrarEliminadas)
            {
                query = query.Where(c => c.Activo);
            }

            if (!string.IsNullOrEmpty(estadoFiltro))
            {
                query = query.Where(c => c.Estado == estadoFiltro);
            }

            var camionetas = await query
                .OrderBy(c => c.Patente)
                .ToListAsync();

            ViewBag.EstadoFiltro = estadoFiltro;
            ViewBag.Estados = new[] { "Disponible", "EnArriendo", "EnMantencion" };
            ViewBag.MostrarEliminadas = mostrarEliminadas;
            
            return View(camionetas);
        }

        // GET: Camionetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var camioneta = await _context.Camionetas
                .Include(c => c.Mantenciones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (camioneta == null)
                return NotFound();

            // Obtener historial
            ViewBag.Historial = await _context.Historial
                .Where(h => h.CamionetaId == id)
                .OrderByDescending(h => h.Fecha)
                .Take(10)
                .ToListAsync();

            return View(camioneta);
        }

        // GET: Camionetas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camionetas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Camioneta camioneta)
        {
            if (ModelState.IsValid)
            {
                camioneta.FechaRegistro = DateTime.Now;
                camioneta.Estado = "Disponible";
                
                _context.Add(camioneta);
                await _context.SaveChangesAsync();

                // Registrar en historial
                var historial = new HistorialCamioneta
                {
                    CamionetaId = camioneta.Id,
                    Accion = "Creacion",
                    Motivo = "Nueva camioneta agregada al sistema",
                    EstadoNuevo = "Disponible",
                    KilometrajeNuevo = camioneta.Kilometraje
                };
                _context.Historial.Add(historial);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Camioneta creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(camioneta);
        }

        // GET: Camionetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null)
                return NotFound();

            return View(camioneta);
        }

        // POST: Camionetas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Camioneta camioneta)
        {
            if (id != camioneta.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Camionetas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                    
                    _context.Update(camioneta);
                    await _context.SaveChangesAsync();

                    // Registrar cambios en historial
                    if (original != null && original.Kilometraje != camioneta.Kilometraje)
                    {
                        var historial = new HistorialCamioneta
                        {
                            CamionetaId = camioneta.Id,
                            Accion = "Actualizacion",
                            Motivo = "Edición manual",
                            KilometrajeAnterior = original.Kilometraje,
                            KilometrajeNuevo = camioneta.Kilometraje
                        };
                        _context.Historial.Add(historial);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Success"] = "Camioneta actualizada exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionetaExists(camioneta.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(camioneta);
        }

        // GET: Camionetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var camioneta = await _context.Camionetas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (camioneta == null)
                return NotFound();

            return View(camioneta);
        }

        // POST: Camionetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            
            if (camioneta == null)
            {
                TempData["Error"] = "Camioneta no encontrada";
                return RedirectToAction(nameof(Index));
            }

            // VALIDACIÓN: Solo se puede eliminar si está Disponible
            if (camioneta.Estado != "Disponible")
            {
                TempData["Error"] = $"No se puede desactivar la camioneta {camioneta.Patente} porque está en estado '{camioneta.Estado}'. Primero cámbiala a 'Disponible'.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                // SOFT DELETE: Solo marcar como inactivo
                camioneta.Activo = false;
                camioneta.FechaEliminacion = DateTime.Now;
                
                _context.Update(camioneta);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = $"Camioneta {camioneta.Patente} desactivada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al desactivar: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        // Método para restaurar una camioneta eliminada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(int id)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            
            if (camioneta == null)
            {
                TempData["Error"] = "Camioneta no encontrada";
                return RedirectToAction(nameof(Index), new { mostrarEliminadas = true });
            }

            try
            {
                camioneta.Activo = true;
                camioneta.FechaEliminacion = null;
                
                _context.Update(camioneta);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = $"Camioneta {camioneta.Patente} restaurada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al restaurar: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index), new { mostrarEliminadas = true });
        }

        // GET: Camionetas/CambiarEstado/5
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null)
                return NotFound();

            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null)
                return NotFound();

            ViewBag.Estados = new[] { "Disponible", "EnArriendo", "EnMantencion" };
            return View(camioneta);
        }

        // POST: Camionetas/CambiarEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado, string? motivo)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null)
                return NotFound();

            var estadoAnterior = camioneta.Estado;
            camioneta.Estado = nuevoEstado;

            // Registrar en historial
            var historial = new HistorialCamioneta
            {
                CamionetaId = camioneta.Id,
                Accion = "CambioEstado",
                Motivo = motivo ?? "Cambio manual de estado",
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = nuevoEstado
            };
            _context.Historial.Add(historial);
            
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Estado cambiado de {estadoAnterior} a {nuevoEstado}";
            return RedirectToAction(nameof(Index));
        }

        private bool CamionetaExists(int id)
        {
            return _context.Camionetas.Any(e => e.Id == id);
        }
    }
}