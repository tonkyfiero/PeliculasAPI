using Peliculas.API.Validaciones;

namespace Peliculas.API.Dtos
{
    public class ActorCreacionDto
    {
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        [MaxSizeFileValidation(maxSizeFileInMb:5)]
        [TypeFileValidation(tipoArchivo:GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
