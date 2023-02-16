using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Dtos
{
    public class ActorPatchDto
    {
        [Required]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
