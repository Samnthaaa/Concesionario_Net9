using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;

namespace CrudRazorApp.Pages.Reservas
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

        [BindProperty]
        public ReservaAuto Reserva { get; set; } = new ReservaAuto();

        public SelectList AutosList { get; set; } = null!;
        public SelectList ConductoresList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.ReservasAuto.FindAsync(id);
            if (reserva == null) return NotFound();

            Reserva = reserva;

            await LoadSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return Page();
            }

            // Verificar solapamientos de reservas para el mismo auto
            var overlap = await _context.ReservasAuto.AnyAsync(r =>
                r.AutoId == Reserva.AutoId &&
                r.Id != Reserva.Id &&
                r.FechaInicio < Reserva.FechaFin &&
                Reserva.FechaInicio < r.FechaFin);

            if (overlap)
            {
                ModelState.AddModelError(string.Empty, "El auto ya está reservado en las fechas seleccionadas.");
                await LoadSelectLists();
                return Page();
            }

            _context.Attach(Reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReservaExists(Reserva.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Index");
        }

        private async Task LoadSelectLists()
        {
            var autos = await _context.Autos.AsNoTracking().ToListAsync();
            var conductores = await _context.Conductores.AsNoTracking().ToListAsync();

            AutosList = new SelectList(autos, "Id", "Placa");
            ConductoresList = new SelectList(conductores, "Id", "Nombre");
        }

        private async Task<bool> ReservaExists(int id)
        {
            return await _context.ReservasAuto.AnyAsync(e => e.Id == id);
        }
    }
}