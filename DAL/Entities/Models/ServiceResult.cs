using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Models
{
    public class ServiceResult
    {
        public ResultType Type { get; set; }
        public string Message { get; set; }
        public ServiceResult(ResultType resultType, string message)
        {
            Type = resultType;
            Message = message;
        }
    }
}
