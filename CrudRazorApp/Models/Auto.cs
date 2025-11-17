using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("Auto")]
    public class Auto
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("marca")]
        [MaxLength(50)]
        public string? Marca { get; set; }

        [Column("modelo")]
        [MaxLength(50)]
        public string? Modelo { get; set; }

        [Column("año")]
        public int? Anio { get; set; }

        [Column("color")]
        [MaxLength(50)]
        public string? Color { get; set; }

        [Column("placa")]
        [MaxLength(20)]
        public string? Placa { get; set; }

        // Navegación
        public ICollection<ReservaAuto>? Reservas { get; set; }
    }
}