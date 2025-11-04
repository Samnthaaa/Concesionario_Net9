using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("ReporteVehiculo")]
    public class ReporteVehiculo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idAuto")]
        public int AutoId { get; set; }

        [Column("fechareporte")]
        public DateTime FechaReporte { get; set; }

        [Column("kilometraje")]
        public int Kilometraje { get; set; }

        [Column("nivelgasolina")]
        public int NivelGasolina { get; set; }

        [Column("totalVentas", TypeName = "decimal(10,2)")]
        public decimal TotalVentas { get; set; }

        [Column("descripcion")]
        [MaxLength(200)]
        public string? Descripcion { get; set; }

        [ForeignKey(nameof(AutoId))]
        public Auto? Auto { get; set; }
    }
}