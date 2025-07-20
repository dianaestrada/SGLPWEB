using SGLPWEB.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int UsuarioId { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    [EmailAddress]
    public string Correo { get; set; }

    public int RolId { get; set; }
    public Rol Rol { get; set; }

    [Required]
    public string ClaveHash { get; set; }

    public bool Activo { get; set; } = true;
}