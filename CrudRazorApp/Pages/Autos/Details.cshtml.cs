using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Autos
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) => _context = context;

        public Auto? Auto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Auto = await _context.Autos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (Auto == null) return NotFound();
            return Page();
        }
    }
}