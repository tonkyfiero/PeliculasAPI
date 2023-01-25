using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Peliculas.API.Servicios.Files.Azure
{
    public class ManageFilesAzure : IManageFiles
    {
        private readonly string connectionString;
        public ManageFilesAzure(IConfiguration configuration )
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task DeleteFile(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();

            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await DeleteFile(ruta, contenedor);
            return await SaveFile(contenido,extension,contenedor,contentType);
        }

        public async Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivoNombre);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders=blobHttpHeader;
            await blob.UploadAsync(new BinaryData(contenido),blobUploadOptions);
            return blob.Uri.ToString();
        }
    }
}
