using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Conductor = await _context.Conductores.FindAsync(id);

            if (Conductor == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var conductor = await _context.Conductores.FindAsync(id);

            if (conductor != null)
            {
                _context.Conductores.Remove(conductor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}