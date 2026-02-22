using System;
using System.ComponentModel.DataAnnotations;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(75)]
        public string Nombre { get; set; }

        [Required, StringLength(75)]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        [Display(Name = "Última conexión")]
        public DateTime? UltimaConexion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        // FK
        public int RolId { get; set; } = 1;

        public virtual Rol Rol { get; set; }
    }
}