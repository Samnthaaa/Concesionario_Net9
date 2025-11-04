using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Autos
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Auto Auto { get; set; } = new Auto();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Autos.Add(Auto);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { success = true, message = $"El auto {Auto.Marca} {Auto.Modelo} se registró exitosamente" });
            }
            catch (Exception ex)
            {
                return RedirectToPage("./Index", new { error = true, message = "No se pudo registrar el auto. Intenta de nuevo." });
            }
        }
    }
}