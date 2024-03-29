﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Inspira_Libertad.Azure
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private readonly string connectionString;
        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task BorrarArchivo(string contenedor, string ruta)
        {
            if(string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();

            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);

            await blob.DeleteIfExistsAsync();

        }

        public async Task<string> EditarArchivo(byte[] contenido, string contenedor, string extension, string contentType, string ruta)
        {
            await BorrarArchivo(contenedor, ruta);
            return await GuardarArchivo(contenido, contenedor, extension, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string contenedor, string extension, string contentType)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(nombreArchivo);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeaders;

            await blob.UploadAsync(new BinaryData(contenido), blobUploadOptions);

            return blob.Uri.ToString();
        }
    }
}
