using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Dtos
{
    public class ActorDto
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string UrlFoto { get; set; }
    }
}
