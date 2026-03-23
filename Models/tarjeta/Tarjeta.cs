using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frontek_Full_Web_E_Commerce.Models.Tarjeta
{
    public class Tarjeta
    {
        [Key, ForeignKey("Usuario")]
        public string IdUsuario { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
        [Required]
        [Column("NumeroTarjeta")]
        public string TarjetaEncriptada { get; set; }
        [Required]
        [Column("CCV")]
        public string CCVEncriptado { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidarFecha]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime? FechaVencimiento { get; set; } 

        [Required]
        [StringLength(50)]
        [Display(Name = "Propietario")]
        public string Propietario { get; set; }

        [NotMapped]
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"^\d{16}$")]
        [Display(Name = "Número de Tarjeta")]
        public string NumeroTarjeta
        {
            get => string.IsNullOrEmpty(TarjetaEncriptada) ? null : CryptoHelper.Desencriptar(TarjetaEncriptada);
            set => TarjetaEncriptada = CryptoHelper.Encriptar(value);
        }

        [NotMapped]
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"^\d{3}$")]
        [Display(Name = "CCV")]
        public string CCV
        {
            get => string.IsNullOrEmpty(CCVEncriptado) ? null : CryptoHelper.Desencriptar(CCVEncriptado);
            set => CCVEncriptado = CryptoHelper.Encriptar(value);
        }
    }
}