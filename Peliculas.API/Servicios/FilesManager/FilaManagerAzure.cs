using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Runtime.Serialization;

namespace Peliculas.API.Servicios.FilesManager
{
    public class FilaManagerAzure : IFileManager
    {
        private readonly string connectionString;

        public FilaManagerAzure(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task Delete(string ruta, string contenedor)
        {
            if(string.IsNullOrWhiteSpace(ruta)) 
            {
                return;
            }
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var ArchivoName = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(ArchivoName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> Edit(byte[] contenido, string contenedor, string extension, string contentType, string ruta)
        {
            await Delete(ruta,contenedor);
            return await Save(contenido, contenedor, extension, contentType);
        }

        public async Task<string> Save(byte[] contenido, string contenedor, string extension, string contentType)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            string ArchivoName = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(ArchivoName);
            var blobUploadOptions = new BlobUploadOptions();
            var blobHeaders = new BlobHttpHeaders();
            blobHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHeaders;

            await blob.UploadAsync(new BinaryData(contenido),blobUploadOptions);

            return blob.Uri.ToString();
        }
    }
}
