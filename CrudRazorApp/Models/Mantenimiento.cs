using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("Mantenimiento")]
    public class Mantenimiento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idAuto")]
        public int AutoId { get; set; }

        // en la BD este campo se llama idEmpleado
        [Column("idEmpleado")]
        public int ConductorId { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("tipo")]
        [MaxLength(100)]
        public string? Tipo { get; set; }

        // Observaciones existe en tu script para Mantenimiento; lo dejo mapeado
        [Column("observaciones")]
        [MaxLength(200)]
        public string? Observaciones { get; set; }

        [Column("costo", TypeName = "decimal(10,2)")]
        public decimal Costo { get; set; }

        [ForeignKey(nameof(AutoId))]
        public Auto? Auto { get; set; }

        [ForeignKey(nameof(ConductorId))]
        public Conductor? Conductor { get; set; }
    }
}