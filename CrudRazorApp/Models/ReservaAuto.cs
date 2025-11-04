using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("ReservaAuto")]
    public class ReservaAuto
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("idAuto")]
        public int AutoId { get; set; }

        [Column("idConductor")]
        public int ConductorId { get; set; }

        [Column("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Column("fechaFin")]
        public DateTime FechaFin { get; set; }

        // Navegación
        [ForeignKey(nameof(AutoId))]
        public Auto? Auto { get; set; }

        [ForeignKey(nameof(ConductorId))]
        public Conductor? Conductor { get; set; }
    }
}