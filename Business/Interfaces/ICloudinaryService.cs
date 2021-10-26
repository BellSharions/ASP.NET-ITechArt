using System.IO;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(string fileName, Stream stream);
        Task<string> DeleteImage(string imageUrl);
    }
}
