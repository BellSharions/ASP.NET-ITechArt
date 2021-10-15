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

        public async Task<string> UploadImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = ""
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult.SecureUrl.AbsoluteUri.ToString();
        }
    }
}
