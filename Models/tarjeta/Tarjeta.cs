using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frontek_Full_Web_E_Commerce.Models.Utils;

namespace Frontek_Full_Web_E_Commerce.Models.Tarjeta
{
    public class Tarjeta
    {
        //Esto lo hice asi para asegurar 1 a 1
        [Key, ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }

        //Esto parece un mucho pero traté de meter la mayor cantidad de validaciones posibles para encriptar bien

        //ENCRIPTADOS 

        [NotMapped]
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"^\d{16}$")]
        [Display(Name = "Número de Tarjeta")]
        public string NumeroTarjeta 
        {
            get => CryptoHelper.Desencriptar(TarjetaEncriptada);
            set => TarjetaEncriptada = CryptoHelper.Encriptar(value);
        }
        
        [Required]
        [Column("NumeroTarjeta")]
        public string TarjetaEncriptada { get; set; }

        [Required]
        [ValidarFecha]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [NotMapped]
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"^\d{3}$")]
        [Display(Name = "CCV")]
        public string CCV
        {
            get => CryptoHelper.Desencriptar(CCVEncriptado);
            set => CCVEncriptado = CryptoHelper.Encriptar(value);

        }

        [Required]
        [Column("CCV")]
        public string CCVEncriptado { get; set; }

        //FIN ENCRIPTADOS

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Propietario")]
        public string Propietario { get; set; }

    }
}

