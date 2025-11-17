using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public int ReservasCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Conductor = await _context.Conductores
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Conductor == null) return NotFound();

            // Contar registros relacionados
            ReservasCount = await _context.ReservasAuto
                .CountAsync(r => r.ConductorId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var conductor = await _context.Conductores.FindAsync(id);

            if (conductor != null)
            {
                // Verificar si tiene registros relacionados
                var hasReservas = await _context.ReservasAuto.AnyAsync(r => r.ConductorId == id);

                if (hasReservas)
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el conductor porque tiene reservas asociadas.";
                    return RedirectToPage("./Index");
                }

                _context.Conductores.Remove(conductor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}