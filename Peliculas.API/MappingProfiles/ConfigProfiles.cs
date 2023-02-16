using AutoMapper;
using Peliculas.API.Dtos;
using Peliculas.API.Entidades;

namespace Peliculas.API.MappingProfiles
{
    public class ConfigProfiles : Profile
    {
        public ConfigProfiles()
        {
            CreateMap<Genero,GeneroDto>().ReverseMap();
            CreateMap<GeneroCreacionDto, Genero>();
            CreateMap<ActorDto,Actor>().ReverseMap();
            CreateMap<ActorCreacionDto, Actor>()
                .ForMember(x => x.UrlFoto, op => op.Ignore());
            CreateMap<ActorPatchDto, Actor>().ReverseMap();
        }
    }
}
