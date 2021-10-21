using Business.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly string _name;
        private readonly string _key;
        private readonly string _secret;

        private readonly Cloudinary _cloudinary;

        public CloudinaryService(CloudinaryOptions options)
        {
            _name = options.CloudName;
            _key = options.ApiKey;
            _secret = options.ApiSecret;
            _cloudinary = new Cloudinary(new Account(
                _name,
                _key,
                _secret
                ));
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImage(string fileName, Stream stream)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = ""
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            await stream.DisposeAsync();
            return uploadResult.SecureUrl.AbsoluteUri.ToString();
        }

        public async Task<string> DeleteImage(string publicId)
        {
            var uploadResult = new DeletionResult();
            using (var stream = new MemoryStream())
            {

                var uploadParams = new DeletionParams(publicId);

                uploadResult = await _cloudinary.DestroyAsync(uploadParams);
            }

            return uploadResult.StatusCode.ToString();
        }
    }
}
