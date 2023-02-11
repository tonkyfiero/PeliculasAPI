using Microsoft.EntityFrameworkCore;

namespace Peliculas.API.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarPaginasHeader<T>(this HttpContext httpContext,IQueryable<T> queryable,int RegistrosPorPagina)
        {
            double totalRegistros = await queryable.CountAsync();
            double paginas = Math.Ceiling(totalRegistros / RegistrosPorPagina);
            httpContext.Response.Headers.Add("paginas", paginas.ToString());            
        }
    }
}
