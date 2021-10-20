using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatingRepository : IRepository<ProductRating>
    {
        Task RecalculateRating(int id);
    }
}
