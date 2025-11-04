using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CrudRazorApp.Data;
using CrudRazorApp.Models;
using CrudRazorApp.Services;

namespace CrudRazorApp.Pages.Autos
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

        public IList<Auto> Autos { get; set; } = new List<Auto>();

        public async Task OnGetAsync()
        {
            Autos = await _context.Autos.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDownloadPdfAsync(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                return RedirectToPage();
            }

            var autos = await _context.Autos
                .Where(a => selectedIds.Contains(a.Id))
                .AsNoTracking()
                .ToListAsync();

            var pdfBytes = _pdfService.GenerateAutosPdf(autos);

            return File(pdfBytes, "application/pdf", $"Reporte_Autos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
    }
}