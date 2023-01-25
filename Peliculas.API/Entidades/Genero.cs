using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
