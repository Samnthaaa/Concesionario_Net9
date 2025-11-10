using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Asegúrate de tener este using
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Conductores
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Conductor? Conductor { get; set; }

        // NUEVO: Propiedades para contar registros relacionados
        public int ReservasCount { get; set; }
        public int MantenimientosCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Conductor = await _context.Conductores.FindAsync(id);

            if (Conductor == null) return NotFound();

            // NUEVO: Contar registros relacionados
            ReservasCount = await _context.ReservasAuto.CountAsync(r => r.ConductorId == id);
            MantenimientosCount = await _context.Mantenimientos.CountAsync(m => m.ConductorId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            // MODIFICADO: Se envuelve la lógica en un try-catch y se añade validación
            var conductor = await _context.Conductores.FindAsync(id);
            if (conductor == null) return NotFound();

            try
            {
                // NUEVO: Validar si existen registros relacionados
                var hasRelated = await _context.ReservasAuto.AnyAsync(r => r.ConductorId == id) ||
                                 await _context.Mantenimientos.AnyAsync(m => m.ConductorId == id);

                if (hasRelated)
                {
                    // Si tiene, redirigir con error, igual que en Autos
                    return RedirectToPage("./Index", new
                    {
                        error = true,
                        message = "No se puede eliminar el conductor porque tiene reservas o mantenimientos asociados. Elimina primero esos registros."
                    });
                }

                _context.Conductores.Remove(conductor);
                await _context.SaveChangesAsync();

                // NUEVO: Redirigir con mensaje de éxito
                return RedirectToPage("./Index", new
                {
                    success = true,
                    message = $"Conductor {conductor.Nombre} {conductor.Apellido} eliminado exitosamente."
                });
            }
            catch (DbUpdateException)
            {
                // NUEVO: Manejo de error por si acaso (ej. restricción de BD)
                return RedirectToPage("./Index", new
                {
                    error = true,
                    message = "Error al eliminar el conductor. Verifica que no tenga registros relacionados."
                });
            }
        }
    }
}