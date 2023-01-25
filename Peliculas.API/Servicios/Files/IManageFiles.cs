namespace Peliculas.API.Servicios.Files
{
    public interface IManageFiles
    {
        Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType);
        Task<string> EditFile(byte[] contenido, string extension, string contenedor, string ruta, string contentType);
        Task DeleteFile(string ruta, string contenedor);
    }
}
