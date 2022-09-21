using ChatApp_xamarin.Utils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using System;
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


        private string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=trankhoi;AccountKey=D7JsgZ7nPCPC8LOPVF34k5b8cLCRoct10CrZt+dPCjLY+v0Z4gEnheUH+cyaz3pAP4LSvdYYbMkv+AStWdCfdg==;EndpointSuffix=core.windows.net";

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

        public async Task<string> UploadMessageImage(MediaFile file)
        {
            string filePath = file.Path;


            // Gets a reference to the images container
            var container = _blClient.GetContainerReference("messages");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            // Uploads the image to the blob storage
            var now = DateTime.Now;
            var imageBlob = container.GetBlockBlobReference($"{now}.png");

            using (var fileStream = File.OpenRead(filePath))
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
                return $"https://trankhoi.blob.core.windows.net/messages/{now}.png";
            }
        }

        public async Task<string> UploadGroupImage(string roomId, MediaFile file)
        {
            string filePath = file.Path;


            // Gets a reference to the images container
            var container = _blClient.GetContainerReference("rooms");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            // Uploads the image to the blob storage
            var imageBlob = container.GetBlockBlobReference($"{roomId}.png");

            using (var fileStream = File.OpenRead(filePath))
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
                return $"https://trankhoi.blob.core.windows.net/rooms/{roomId}.png";
            }
        }
    }
}
