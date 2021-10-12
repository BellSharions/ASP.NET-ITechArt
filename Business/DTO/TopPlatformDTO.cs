using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class TopPlatformDTO
    {
        public TopPlatformDTO(string platform, int count)
        {
            Platform = platform;
            Count = count;
        }

        public string Platform { get; set; }
        public int Count {  get; set; }
    }
}
