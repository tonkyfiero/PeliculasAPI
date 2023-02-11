namespace Peliculas.API.Servicios.FilesManager
{
    public class FileManagerLocal : IFileManager
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileManagerLocal(IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task Delete(string ruta, string contenedor)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(env.WebRootPath,contenedor,nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }
            return Task.FromResult(0);
        }

        public async Task<string> Edit(byte[] contenido, string contenedor, string extension, string contentType, string ruta)
        {
            await Delete(ruta,contenedor);
            return await Save(contenido, contenedor, extension, contentType);
        }

        public async Task<string> Save(byte[] contenido, string contenedor, string extension, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string ruta = Path.Combine(folder, nombreArchivo);

            await File.WriteAllBytesAsync(ruta,contenido);

            var urlPC = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlToDB = Path.Combine(urlPC, contenedor, nombreArchivo).Replace('\\','/');
            return urlToDB;
        }
    }
}
