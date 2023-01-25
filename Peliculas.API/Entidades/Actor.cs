using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string UrlFoto { get; set; }
    }
}
