using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Validaciones
{
    public class MaxSizeFileValidation : ValidationAttribute
    {
        private readonly int maxSizeFileInMb;

        public MaxSizeFileValidation(int maxSizeFileInMb)
        {
            this.maxSizeFileInMb = maxSizeFileInMb;
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

            if (formFile.Length > maxSizeFileInMb * 1024* 1024)
            {
                return new ValidationResult($"El peso maximo del archivo es de {maxSizeFileInMb}mb");
            }

            return ValidationResult.Success;
            
        }
    }
}
