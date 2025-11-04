using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CrudRazorApp.Data;
using CrudRazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudRazorApp.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public ReservaAuto Reserva { get; set; } = new ReservaAuto();

        public SelectList AutosList { get; set; }
        public SelectList ConductoresList { get; set; }

        public async Task OnGetAsync()
        {
            // Establecer la fecha de hoy como fecha de inicio (con hora actual)
            Reserva.FechaInicio = DateTime.Now;

            // Establecer la fecha de fin como un día después (puedes ajustar esto)
            Reserva.FechaFin = DateTime.Now.AddDays(1);

            AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa");
            ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa");
                ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre");
                return Page();
            }

            // Opcional: verificar solapamientos de reservas para el mismo auto
            var overlap = await _context.ReservasAuto.AnyAsync(r =>
                r.AutoId == Reserva.AutoId &&
                r.Id != Reserva.Id &&
                r.FechaInicio < Reserva.FechaFin &&
                Reserva.FechaInicio < r.FechaFin);

            if (overlap)
            {
                ModelState.AddModelError(string.Empty, "El auto ya está reservado en las fechas seleccionadas.");
                AutosList = new SelectList(await _context.Autos.AsNoTracking().ToListAsync(), "Id", "Placa");
                ConductoresList = new SelectList(await _context.Conductores.AsNoTracking().ToListAsync(), "Id", "Nombre");
                return Page();
            }

            _context.ReservasAuto.Add(Reserva);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}