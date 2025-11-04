using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Conductores
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Conductor> Conductores { get; set; } = new List<Conductor>();

        public async Task OnGetAsync()
        {
            Conductores = await _context.Conductores.AsNoTracking().ToListAsync();
        }
    }
}