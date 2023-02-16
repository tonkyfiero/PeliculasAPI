using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.API.Context;
using Peliculas.API.Dtos;
using Peliculas.API.Entidades;
using Peliculas.API.Helpers;
using Peliculas.API.Servicios.FilesManager;

namespace Peliculas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileManager manageFiles;
        private readonly string contenedor = "actores";

        public ActorController(ApplicationDbContext context,IMapper mapper, IFileManager manageFiles)
        {
            this.context = context;
            this.mapper = mapper;            
            this.manageFiles = manageFiles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> Get([FromQuery] PaginacionDto paginacion)
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarPaginasHeader(queryable, paginacion.RegistrosPorPagina);
            var entidades = await queryable.Paginacion(paginacion).ToListAsync();
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
                    entidad.UrlFoto = await manageFiles.Save(contenido,contenedor,extension,contentType);
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
                    var contentType = creacionDto.Foto.ContentType;
                    actorDB.UrlFoto = await manageFiles.Edit(contenido,contenedor,extension,contentType,actorDB.UrlFoto);
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

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDto> jsonPatchDocument)
        {
            var entidadDb = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDb == null)
            {
                return NotFound();
            }

            var entidadDto = mapper.Map<ActorPatchDto>(entidadDb);
            jsonPatchDocument.ApplyTo(entidadDto,ModelState);

            var isValid = TryValidateModel(entidadDto);

            if (!isValid) {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDto, entidadDb);

            await context.SaveChangesAsync();            

            return NoContent();
        }
    }
}
