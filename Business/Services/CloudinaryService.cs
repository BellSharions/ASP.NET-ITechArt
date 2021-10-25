using Business.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly string _name;
        private readonly string _key;
        private readonly string _secret;

        private readonly string _regexPublicId = @"[a-zA-Z]+([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[eE]([+-]?\d+))?[a-zA-Z]+\.";

        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinaryOptions> options)
        {
            _name = options.Value.CloudName;
            _key = options.Value.ApiKey;
            _secret = options.Value.ApiSecret;
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

        public async Task<string> DeleteImage(string imageUrl)
        {
            var publicId = Regex.Matches(imageUrl, _regexPublicId)[0].Value.Split(".")[0];
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
