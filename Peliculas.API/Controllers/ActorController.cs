using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.API.Context;
using Peliculas.API.Dtos;
using Peliculas.API.Entidades;
using Peliculas.API.Servicios.Files;

namespace Peliculas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IManageFiles manageFiles;
        private readonly string contenedor = "actores";

        public ActorController(ApplicationDbContext context,IMapper mapper, IManageFiles manageFiles)
        {
            this.context = context;
            this.mapper = mapper;
            this.manageFiles = manageFiles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> Get()
        {
            var entidades = await context.Actores.ToListAsync();
            return mapper.Map<List<ActorDto>>(entidades);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDto>> GetById(int id)
        {
            var entidad = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDto>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]ActorCreacionDto creacionDto)
        {
            var entidad = mapper.Map<Actor>(creacionDto);
           
            if (creacionDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creacionDto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(creacionDto.Foto.FileName);
                    var contentType = creacionDto.Foto.ContentType;
                    entidad.UrlFoto = await manageFiles.SaveFile(contenido, extension, contenedor, contentType);
                }
            }

            context.Add(entidad);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDto creacionDto)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(creacionDto, actorDB);

            if (creacionDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await creacionDto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(creacionDto.Foto.FileName);
                    actorDB.UrlFoto = await manageFiles.EditFile(contenido,extension,contenedor,actorDB.UrlFoto,creacionDto.Foto.ContentType);
                }

            }
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
