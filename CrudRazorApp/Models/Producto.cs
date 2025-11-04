using System.ComponentModel.DataAnnotations;

namespace CrudRazorApp.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 999999999)]
        public decimal Precio { get; set; }
    }
}
