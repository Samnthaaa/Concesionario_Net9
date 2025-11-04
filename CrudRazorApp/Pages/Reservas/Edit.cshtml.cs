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

        public SelectList AutosList { get; set; }
        public SelectList ConductoresList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Reserva = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Reserva == null) return NotFound();

            AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa", Reserva.AutoId);
            ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre", Reserva.ConductorId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa", Reserva.AutoId);
                ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre", Reserva.ConductorId);
                return Page();
            }

            // Verificar solapamientos de reservas
            var overlap = await _context.ReservasAuto.AnyAsync(r =>
                r.AutoId == Reserva.AutoId &&
                r.Id != Reserva.Id &&
                r.FechaInicio < Reserva.FechaFin &&
                Reserva.FechaInicio < r.FechaFin);

            if (overlap)
            {
                ModelState.AddModelError(string.Empty, "El auto ya está reservado en las fechas seleccionadas.");
                AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa", Reserva.AutoId);
                ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre", Reserva.ConductorId);
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

        private async Task<bool> ReservaExists(int id)
        {
            return await _context.ReservasAuto.AnyAsync(e => e.Id == id);
        }
    }
}