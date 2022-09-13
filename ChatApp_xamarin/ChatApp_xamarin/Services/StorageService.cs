using Azure.Core;
using Azure.Storage.Blobs;
using ChatApp_xamarin.Models;
using ChatApp_xamarin.Utils;
using Firebase.Database;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace ChatApp_xamarin.Services
{
    public class StorageService
    {
        private static StorageService _ins;
        public static StorageService ins
        {
            get
            {
                if (_ins == null)
                    _ins = new StorageService();
                return _ins;
            }
            set { _ins = value; }
        }


        private string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=trankhoi;AccountKey=9KOUw/zYJ2mgGFzDZHYV6D2dsRqXSerLI13gYHWBeIfY1TsCp6yzcFFixSMMpgnF6jiEOnJMFBZF+AStALYByw==;EndpointSuffix=core.windows.net";

        private CloudBlobClient _blClient;

        StorageService()
        {
            var account = CloudStorageAccount.Parse(storageConnectionString);
            _blClient = account.CreateCloudBlobClient();
        }

        public async Task<string> UploadUserAvatar(MediaFile file)
        {
            string filePath = file.Path;


            // Gets a reference to the images container
            var container = _blClient.GetContainerReference("users");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            // Uploads the image to the blob storage
            var imageBlob = container.GetBlockBlobReference($"{GlobalData.ins.currentUser.id}.png");

            using (var fileStream = File.OpenRead(filePath))
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
                return $"https://trankhoi.blob.core.windows.net/users/{GlobalData.ins.currentUser.id}.png";
            }
        }
    }
}
