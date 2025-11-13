using CrudRazorApp.Data;
using CrudRazorApp.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrudRazorApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Registrar nuevo usuario
        public async Task<(bool Success, string Message)> RegisterAsync(string username, string email, string password, string? nombreCompleto = null)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                    return (false, "El nombre de usuario debe tener al menos 3 caracteres");

                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    return (false, "El email no es válido");

                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    return (false, "La contraseña debe tener al menos 6 caracteres");

                // Verificar si el usuario ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
                    return (false, "El nombre de usuario ya está en uso");

                if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                    return (false, "El email ya está registrado");

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Username = username.Trim(),
                    Email = email.Trim().ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    NombreCompleto = nombreCompleto?.Trim(),
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return (true, "Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar usuario: {ex.Message}");
            }
        }

        // Login de usuario
        public async Task<(bool Success, Usuario? Usuario, string Message)> LoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
                    return (false, null, "Usuario y contraseña son requeridos");

                // Buscar usuario por username o email
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u =>
                        u.Username.ToLower() == usernameOrEmail.ToLower() ||
                        u.Email.ToLower() == usernameOrEmail.ToLower());

                if (usuario == null)
                    return (false, null, "Usuario o contraseña incorrectos");

                if (!usuario.Activo)
                    return (false, null, "Usuario inactivo. Contacta al administrador");

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                    return (false, null, "Usuario o contraseña incorrectos");

                // Actualizar último acceso
                usuario.UltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();

                return (true, usuario, "Login exitoso");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al iniciar sesión: {ex.Message}");
            }
        }

        // Validar email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Cambiar contraseña
        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(userId);
                if (usuario == null)
                    return (false, "Usuario no encontrado");

                if (!BCrypt.Net.BCrypt.Verify(oldPassword, usuario.PasswordHash))
                    return (false, "La contraseña actual es incorrecta");

                if (newPassword.Length < 6)
                    return (false, "La nueva contraseña debe tener al menos 6 caracteres");

                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return (true, "Contraseña actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al cambiar contraseña: {ex.Message}");
            }
        }
    }
}