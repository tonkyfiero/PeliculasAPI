using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Validaciones
{
    public class TypeFileValidation : ValidationAttribute
    {
        private readonly string[] tiposValidos;

        public TypeFileValidation(string[] tiposValidos)
        {
            this.tiposValidos = tiposValidos;
        }
        public TypeFileValidation(GrupoTipoArchivo tipoArchivo)
        {
            if (tipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tiposValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser de los siguientes {string.Join(",", tiposValidos)}");
            }

            return ValidationResult.Success;

        }
    }
}
