using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudRazorApp.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Column("email")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Column("passwordHash")]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("nombreCompleto")]
        [MaxLength(100)]
        public string? NombreCompleto { get; set; }

        [Column("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("ultimoAcceso")]
        public DateTime? UltimoAcceso { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;
    }
}