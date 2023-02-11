using Peliculas.API.Dtos;

namespace Peliculas.API.Helpers
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Paginacion<T>(this IQueryable<T> queryable,PaginacionDto paginacionDto)
        {
            return queryable
                    .Skip((paginacionDto.Pagina - 1) * paginacionDto.RegistrosPorPagina)
                    .Take(paginacionDto.RegistrosPorPagina);
        }
    }
}
