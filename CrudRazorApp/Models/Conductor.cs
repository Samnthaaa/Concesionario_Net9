using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("Conductor")]
    public class Conductor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Column("apellido")]
        [MaxLength(100)]
        public string? Apellido { get; set; }

        [Column("fechanacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("telefono")]
        public int? Telefono { get; set; }

        [Column("email")]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Column("licencia")]
        [MaxLength(20)]
        public string? Licencia { get; set; }

        [Column("fechaContratacion")]
        public DateTime? FechaContratacion { get; set; }

        // Navegación
        public ICollection<ReservaAuto>? Reservas { get; set; }
    }
}