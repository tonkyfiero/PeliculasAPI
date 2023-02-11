namespace Peliculas.API.Servicios.FilesManager
{
    public interface IFileManager
    {
        Task<string> Save(byte[] contenido,string contenedor, string extension, string contentType);
        Task<string> Edit(byte[] contenido, string contenedor, string extension, string contentType,string ruta);
        Task Delete(string ruta, string contenedor);
    }
}
