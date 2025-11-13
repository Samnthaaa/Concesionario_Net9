using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages
{
    [Authorize] // PROTEGER LA PÁGINA
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public int AutosCount { get; set; }
        public int ConductoresCount { get; set; }
        public int ReservasCount { get; set; }

        public IList<ReservaAuto> RecentReservas { get; set; } = new List<ReservaAuto>();

        public async Task OnGetAsync()
        {
            AutosCount = await _context.Autos.CountAsync();
            ConductoresCount = await _context.Conductores.CountAsync();
            ReservasCount = await _context.ReservasAuto.CountAsync();

            RecentReservas = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .OrderByDescending(r => r.FechaInicio)
                .Take(6)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}