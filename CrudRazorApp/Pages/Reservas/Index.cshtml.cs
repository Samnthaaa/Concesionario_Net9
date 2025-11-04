using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<ReservaAuto> Reservas { get; set; } = new List<ReservaAuto>();

        public async Task OnGetAsync()
        {
            Reservas = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}