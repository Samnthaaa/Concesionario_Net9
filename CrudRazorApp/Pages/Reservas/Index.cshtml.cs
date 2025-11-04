using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;
using CrudRazorApp.Services;

namespace CrudRazorApp.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly PdfService _pdfService;

        public IndexModel(AppDbContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        public IList<ReservaAuto> Reservas { get; set; } = new List<ReservaAuto>();

        public async Task OnGetAsync()
        {
            Reservas = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDownloadPdfAsync(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                return RedirectToPage();
            }

            var reservas = await _context.ReservasAuto
                .Include(r => r.Auto)
                .Include(r => r.Conductor)
                .Where(r => selectedIds.Contains(r.Id))
                .AsNoTracking()
                .ToListAsync();

            var pdfBytes = _pdfService.GenerateReservasPdf(reservas);

            return File(pdfBytes, "application/pdf", $"Reporte_Reservas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
    }
}