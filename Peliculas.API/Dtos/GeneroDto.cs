using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Dtos
{
    public class GeneroDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
