using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using CrudRazorApp.Models;

namespace CrudRazorApp.Services
{
    public class PdfService
    {
        private static readonly DeviceRgb PrimaryColor = new DeviceRgb(37, 99, 235); // #2563EB
        private static readonly DeviceRgb SecondaryColor = new DeviceRgb(100, 116, 139); // #64748B
        private static readonly DeviceRgb BorderColor = new DeviceRgb(226, 232, 240); // #E2E8F0

        public byte[] GenerateAutosPdf(List<Auto> autos)
        {
            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(40, 40, 40, 40);

            // Membretado
            AddHeader(document, "Reporte de Autos", autos.Count);

            // Tabla
            var table = new Table(UnitValue.CreatePercentArray(new[] { 20f, 20f, 15f, 20f, 25f }))
                .UseAllAvailableWidth()
                .SetMarginTop(20);

            // Encabezados
            AddTableHeader(table, "Marca");
            AddTableHeader(table, "Modelo");
            AddTableHeader(table, "Año");
            AddTableHeader(table, "Color");
            AddTableHeader(table, "Placa");

            // Datos
            foreach (var auto in autos)
            {
                AddTableCell(table, auto.Marca ?? "N/A");
                AddTableCell(table, auto.Modelo ?? "N/A");
                AddTableCell(table, auto.Anio?.ToString() ?? "N/A");
                AddTableCell(table, auto.Color ?? "N/A");
                AddTableCell(table, auto.Placa ?? "N/A");
            }

            document.Add(table);
            AddFooter(document);
            document.Close();

            return memoryStream.ToArray();
        }

        public byte[] GenerateConductoresPdf(List<Conductor> conductores)
        {
            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(40, 40, 40, 40);

            AddHeader(document, "Reporte de Conductores", conductores.Count);

            var table = new Table(UnitValue.CreatePercentArray(new[] { 25f, 25f, 20f, 30f }))
                .UseAllAvailableWidth()
                .SetMarginTop(20);

            AddTableHeader(table, "Nombre");
            AddTableHeader(table, "Apellido");
            AddTableHeader(table, "Teléfono");
            AddTableHeader(table, "Email");

            foreach (var conductor in conductores)
            {
                AddTableCell(table, conductor.Nombre ?? "N/A");
                AddTableCell(table, conductor.Apellido ?? "N/A");
                AddTableCell(table, conductor.Telefono?.ToString() ?? "N/A");
                AddTableCell(table, conductor.Email ?? "N/A");
            }

            document.Add(table);
            AddFooter(document);
            document.Close();

            return memoryStream.ToArray();
        }

        public byte[] GenerateReservasPdf(List<ReservaAuto> reservas)
        {
            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4.Rotate()); // Modo horizontal
            document.SetMargins(40, 40, 40, 40);

            AddHeader(document, "Reporte de Reservas", reservas.Count);

            var table = new Table(UnitValue.CreatePercentArray(new[] { 20f, 20f, 25f, 25f, 10f }))
                .UseAllAvailableWidth()
                .SetMarginTop(20);

            AddTableHeader(table, "Auto");
            AddTableHeader(table, "Conductor");
            AddTableHeader(table, "Fecha Inicio");
            AddTableHeader(table, "Fecha Fin");
            AddTableHeader(table, "Estado");

            foreach (var reserva in reservas)
            {
                var autoInfo = $"{reserva.Auto?.Marca} {reserva.Auto?.Modelo} ({reserva.Auto?.Placa})";
                var conductorInfo = $"{reserva.Conductor?.Nombre} {reserva.Conductor?.Apellido}";
                var estado = DateTime.Now < reserva.FechaInicio ? "Pendiente" :
                            DateTime.Now > reserva.FechaFin ? "Finalizada" : "En Curso";

                AddTableCell(table, autoInfo);
                AddTableCell(table, conductorInfo);
                AddTableCell(table, reserva.FechaInicio.ToString("dd/MM/yyyy HH:mm"));
                AddTableCell(table, reserva.FechaFin.ToString("dd/MM/yyyy HH:mm"));
                AddTableCell(table, estado);
            }

            document.Add(table);
            AddFooter(document);
            document.Close();

            return memoryStream.ToArray();
        }

        private void AddHeader(Document document, string title, int count)
        {
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // Barra superior con color primario
            var headerBar = new Paragraph()
                .SetBackgroundColor(PrimaryColor)
                .SetHeight(8)
                .SetMargin(0)
                .SetPadding(0);
            document.Add(headerBar);

            // Título
            var titleParagraph = new Paragraph("CONCESIONARIO")
                .SetFont(boldFont)
                .SetFontSize(24)
                .SetFontColor(PrimaryColor)
                .SetMarginTop(20)
                .SetMarginBottom(5);
            document.Add(titleParagraph);

            // Subtítulo
            var subtitleParagraph = new Paragraph(title)
                .SetFont(boldFont)
                .SetFontSize(16)
                .SetFontColor(SecondaryColor)
                .SetMarginBottom(5);
            document.Add(subtitleParagraph);

            // Información
            var infoParagraph = new Paragraph($"Total de registros: {count} | Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetFont(regularFont)
                .SetFontSize(10)
                .SetFontColor(SecondaryColor)
                .SetMarginBottom(10);
            document.Add(infoParagraph);

            // Línea separadora
            var separator = new Paragraph()
                .SetBackgroundColor(BorderColor)
                .SetHeight(1)
                .SetMarginTop(10)
                .SetMarginBottom(10);
            document.Add(separator);
        }

        private void AddTableHeader(Table table, string text)
        {
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var cell = new Cell()
                .Add(new Paragraph(text).SetFont(boldFont).SetFontSize(10))
                .SetBackgroundColor(new DeviceRgb(248, 250, 252)) // #F8FAFC
                .SetFontColor(SecondaryColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetPadding(10)
                .SetBorder(new iText.Layout.Borders.SolidBorder(BorderColor, 1));
            table.AddHeaderCell(cell);
        }

        private void AddTableCell(Table table, string text)
        {
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var cell = new Cell()
                .Add(new Paragraph(text).SetFont(regularFont).SetFontSize(9))
                .SetFontColor(new DeviceRgb(30, 41, 59)) // #1E293B
                .SetTextAlignment(TextAlignment.LEFT)
                .SetPadding(8)
                .SetBorder(new iText.Layout.Borders.SolidBorder(BorderColor, 1));
            table.AddCell(cell);
        }

        private void AddFooter(Document document)
        {
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var footerText = new Paragraph($"© {DateTime.Now.Year} Concesionario. Todos los derechos reservados.")
                .SetFont(regularFont)
                .SetFontSize(8)
                .SetFontColor(SecondaryColor)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(30);
            document.Add(footerText);
        }
    }
}