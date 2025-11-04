using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Autos
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Auto> Autos { get; set; } = new List<Auto>();

        public async Task OnGetAsync()
        {
            Autos = await _context.Autos.AsNoTracking().ToListAsync();
        }
    }
}