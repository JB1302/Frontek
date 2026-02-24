using System;
using System.ComponentModel.DataAnnotations;

namespace Frontek_Full_Web_E_Commerce.Models.Tarjeta
{
    public class ValidarFecha : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {

            if (value is DateTime fecha)
            {
                if (fecha < DateTime.Today)
                {
                    return new ValidationResult("No se pueden ingresar tarjetas vencidas.");
                }
            }

            return ValidationResult.Success;
        }

    }
}