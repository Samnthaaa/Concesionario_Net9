using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Autos
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Auto? Auto { get; set; }

        public int ReservasCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Auto = await _context.Autos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (Auto == null) return NotFound();

            // Contar registros relacionados
            ReservasCount = await _context.ReservasAuto
                .CountAsync(r => r.AutoId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var auto = await _context.Autos.FindAsync(id);

            if (auto != null)
            {
                // Verificar si tiene registros relacionados
                var hasReservas = await _context.ReservasAuto.AnyAsync(r => r.AutoId == id);

                if (hasReservas)
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el auto porque tiene reservas asociadas.";
                    return RedirectToPage("./Index");
                }

                _context.Autos.Remove(auto);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}