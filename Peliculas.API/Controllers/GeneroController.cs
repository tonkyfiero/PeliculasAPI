using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.API.Context;
using Peliculas.API.Dtos;
using Peliculas.API.Entidades;

namespace Peliculas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GeneroController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDto>>> Get()
        {
            var generos = await context.Generos.ToListAsync();
            return mapper.Map<List<GeneroDto>>(generos);
        }

        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        public async Task<ActionResult<GeneroDto>> GetById(int id)
        {
            var entidad = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            
            return mapper.Map<GeneroDto>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GeneroCreacionDto creacionDto)
        {
            var entidad = mapper.Map<Genero>(creacionDto);
            context.Add(entidad);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,[FromBody]GeneroCreacionDto creacionDto)
        {
            var entidad = mapper.Map<Genero>(creacionDto);
            entidad.Id = id;
            context.Entry(entidad).State= EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe  = await context.Generos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero { Id = id }); 
            await context.SaveChangesAsync();
            return NoContent();

        }



    }
}
