using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class Rol
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        [Display(Name = "Rol")]
        public string Nombre { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set   ; }
        }
}