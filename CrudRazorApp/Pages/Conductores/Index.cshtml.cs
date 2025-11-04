using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;
using CrudRazorApp.Services;

namespace CrudRazorApp.Pages.Conductores
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

        public IList<Conductor> Conductores { get; set; } = new List<Conductor>();

        public async Task OnGetAsync()
        {
            Conductores = await _context.Conductores.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDownloadPdfAsync(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                return RedirectToPage();
            }

            var conductores = await _context.Conductores
                .Where(c => selectedIds.Contains(c.Id))
                .AsNoTracking()
                .ToListAsync();

            var pdfBytes = _pdfService.GenerateConductoresPdf(conductores);

            return File(pdfBytes, "application/pdf", $"Reporte_Conductores_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
    }
}