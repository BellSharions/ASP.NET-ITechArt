using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Roles
{
    public class Role : IdentityRole<int>
    {
        public Role(string name, string normalizedName)
        {
            Name = name;
            NormalizedName = normalizedName;
            ConcurrencyStamp = (new Random().Next()).ToString();
        }
    }
}
