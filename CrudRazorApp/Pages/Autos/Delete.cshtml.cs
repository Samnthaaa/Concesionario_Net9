// Ejemplo para Autos/Delete.cshtml.cs
using System.Linq;
using System.Threading.Tasks;
using CrudRazorApp.Data;
using CrudRazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrudRazorApp.Pages.Autos
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Auto? Auto { get; set; }

        public int ReservasCount { get; set; }
        public int MantenimientosCount { get; set; }
        public int ReportesCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Auto = await _context.Autos.FindAsync(id);
            if (Auto == null) return NotFound();

            // Contar registros relacionados
            ReservasCount = await _context.ReservasAuto.CountAsync(r => r.AutoId == id);
            MantenimientosCount = await _context.Mantenimientos.CountAsync(m => m.AutoId == id);
            ReportesCount = await _context.ReportesVehiculo.CountAsync(r => r.AutoId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var auto = await _context.Autos.FindAsync(id);
            if (auto == null) return NotFound();

            try
            {
                // Opción 1: Eliminar solo si no tiene registros relacionados
                var hasRelated = await _context.ReservasAuto.AnyAsync(r => r.AutoId == id) ||
                                await _context.Mantenimientos.AnyAsync(m => m.AutoId == id) ||
                                await _context.ReportesVehiculo.AnyAsync(r => r.AutoId == id);

                if (hasRelated)
                {
                    return RedirectToPage("./Index", new
                    {
                        error = true,
                        message = "No se puede eliminar el auto porque tiene reservas, mantenimientos o reportes asociados. Elimina primero esos registros."
                    });
                }

                // Opción 2: Eliminar en cascada manualmente (descomenta si prefieres esto)
                /*
                // Eliminar registros relacionados primero
                var reservas = await _context.ReservasAuto.Where(r => r.AutoId == id).ToListAsync();
                var mantenimientos = await _context.Mantenimientos.Where(m => m.AutoId == id).ToListAsync();
                var reportes = await _context.ReportesVehiculo.Where(r => r.AutoId == id).ToListAsync();

                _context.ReservasAuto.RemoveRange(reservas);
                _context.Mantenimientos.RemoveRange(mantenimientos);
                _context.ReportesVehiculo.RemoveRange(reportes);
                */

                _context.Autos.Remove(auto);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index", new
                {
                    success = true,
                    message = $"Auto {auto.Marca} {auto.Modelo} eliminado exitosamente."
                });
            }
            catch (DbUpdateException)
            {
                return RedirectToPage("./Index", new
                {
                    error = true,
                    message = "Error al eliminar el auto. Verifica que no tenga registros relacionados."
                });
            }
        }
    }
}