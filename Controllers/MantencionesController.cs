using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaMantencion.Data;
using SistemaMantencion.Models;

namespace SistemaMantencion.Controllers
{
    public class MantencionesController : Controller
    {
        private readonly MantencionDbContext _context;

        public MantencionesController(MantencionDbContext context)
        {
            _context = context;
        }

        // GET: Mantenciones
        public async Task<IActionResult> Index(string? estadoFiltro)
        {
            var query = _context.Mantenciones
                .Include(m => m.Camioneta)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estadoFiltro))
            {
                query = query.Where(m => m.Estado == estadoFiltro);
            }

            var mantenciones = await query
                .OrderByDescending(m => m.FechaInicio)
                .ToListAsync();

            ViewBag.EstadoFiltro = estadoFiltro;
            return View(mantenciones);
        }

        // GET: Mantenciones/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Camionetas = new SelectList(
                await _context.Camionetas.ToListAsync(), 
                "Id", 
                "Patente");
            
            ViewBag.TiposMantenciones = new[] { "Preventiva", "Correctiva", "Revision" };
            
            return View();
        }

        // POST: Mantenciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistroMantencion mantencion)
        {
            if (ModelState.IsValid)
            {
                var camioneta = await _context.Camionetas.FindAsync(mantencion.CamionetaId);
                
                if (camioneta != null)
                {
                    mantencion.KilometrajeRegistrado = camioneta.Kilometraje;
                    mantencion.FechaInicio = DateTime.Now;
                    mantencion.Estado = "EnProceso";

                    // Cambiar estado de camioneta
                    var estadoAnterior = camioneta.Estado;
                    camioneta.Estado = "EnMantencion";

                    // Registrar historial
                    var historial = new HistorialCamioneta
                    {
                        CamionetaId = camioneta.Id,
                        Accion = "Mantencion",
                        Motivo = $"Inicio de mantención: {mantencion.TipoMantencion}",
                        EstadoAnterior = estadoAnterior,
                        EstadoNuevo = "EnMantencion",
                        KilometrajeAnterior = camioneta.Kilometraje
                    };

                    _context.Mantenciones.Add(mantencion);
                    _context.Historial.Add(historial);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Mantención registrada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Camionetas = new SelectList(
                await _context.Camionetas.ToListAsync(), 
                "Id", 
                "Patente");
            ViewBag.TiposMantenciones = new[] { "Preventiva", "Correctiva", "Revision" };
            
            return View(mantencion);
        }

        // GET: Mantenciones/Completar/5
        public async Task<IActionResult> Completar(int? id)
        {
            if (id == null)
                return NotFound();

            var mantencion = await _context.Mantenciones
                .Include(m => m.Camioneta)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mantencion == null)
                return NotFound();

            return View(mantencion);
        }

        // POST: Mantenciones/Completar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Completar(int id, decimal? costo, string? observaciones)
        {
            var mantencion = await _context.Mantenciones
                .Include(m => m.Camioneta)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mantencion == null)
                return NotFound();

            mantencion.FechaFin = DateTime.Now;
            mantencion.Estado = "Completada";
            mantencion.Costo = costo;
            mantencion.Observaciones = observaciones;

            // Cambiar estado de camioneta a Disponible
            var camioneta = mantencion.Camioneta;
            camioneta.Estado = "Disponible";
            camioneta.FechaUltimaMantencion = DateTime.Now;

            // Registrar historial
            var historial = new HistorialCamioneta
            {
                CamionetaId = camioneta.Id,
                Accion = "FinMantencion",
                Motivo = $"Mantención completada: {mantencion.TipoMantencion}",
                EstadoAnterior = "EnMantencion",
                EstadoNuevo = "Disponible"
            };

            _context.Historial.Add(historial);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Mantención completada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        // GET: Mantenciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var mantencion = await _context.Mantenciones
                .Include(m => m.Camioneta)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mantencion == null)
                return NotFound();

            return View(mantencion);
        }

        // GET: Mantenciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var mantencion = await _context.Mantenciones
                .Include(m => m.Camioneta)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mantencion == null)
                return NotFound();

            return View(mantencion);
        }

        // POST: Mantenciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mantencion = await _context.Mantenciones.FindAsync(id);
            if (mantencion != null)
            {
                _context.Mantenciones.Remove(mantencion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Mantención eliminada exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}