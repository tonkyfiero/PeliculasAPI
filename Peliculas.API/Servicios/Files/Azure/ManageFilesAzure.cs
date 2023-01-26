using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Peliculas.API.Servicios.Files.Azure
{
    public class ManageFilesAzure : IManageFiles
    {
        private readonly string connectionAzure;
        public ManageFilesAzure(IConfiguration configuration)
        {
            connectionAzure = configuration.GetConnectionString("AzureStorage");
        }

        public async Task DeleteFile(string ruta, string contenedor)
        {
            var cliente = new BlobContainerClient(connectionAzure, contenedor);
            await cliente.CreateIfNotExistsAsync();

            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await DeleteFile(ruta,contenedor);
            return await SaveFile(contenido, extension, contenedor, contentType);
        }

        public async Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var cliente = new BlobContainerClient(connectionAzure, contenedor);
            await cliente.CreateIfNotExistsAsync();
            await cliente.SetAccessPolicyAsync(PublicAccessType.Blob);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(nombreArchivo) ;
            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeaders;
            await blob.UploadAsync(new BinaryData(contenido), blobUploadOptions);
            return blob.Uri.ToString();
            
        }
    }
}
