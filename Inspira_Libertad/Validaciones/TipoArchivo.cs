using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Inspira_Libertad.Validaciones
{
    public class TipoArchivo:ValidationAttribute
    {
        private readonly string[] tiposValidos;

        public TipoArchivo(string[] tiposValidos)
        {
            this.tiposValidos = tiposValidos;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if(formFile == null) {

                return ValidationResult.Success;
            }

            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult("El tipo de archivo debe ser jpg, jpeg o png..");
            }

            return ValidationResult.Success;
        }
    }
}
