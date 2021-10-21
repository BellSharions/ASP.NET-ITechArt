using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(string fileName, Stream stream);
    }
}
