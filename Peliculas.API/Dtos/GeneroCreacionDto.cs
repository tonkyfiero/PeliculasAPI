using System.ComponentModel.DataAnnotations;

namespace Peliculas.API.Dtos
{
    public class GeneroCreacionDto
    {      
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
