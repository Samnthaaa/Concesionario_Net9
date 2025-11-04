using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Reservas
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) => _context = context;

        public ReservaAuto? Reserva { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Reserva = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Reserva == null) return NotFound();

            return Page();
        }
    }
}