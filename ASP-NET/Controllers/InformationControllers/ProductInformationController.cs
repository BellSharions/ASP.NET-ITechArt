using ASP_NET.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{
    [ApiController]
    [Route("api/games")]
    public class ProductInformationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DbSet<Product> Products;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductInformationController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        [Route("top-platforms")]
        [HttpGet]
        public async Task<IActionResult> FindTopPlatforms()
        {
            var topPlatforms =
                from products in _context.Products
                group products by products.Platform into productsGroup
                select new
                {
                    Platform = productsGroup.Key.ToString(),
                    Count = productsGroup.Count(),
                };
            return Ok(topPlatforms.Take(3).ToList().OrderByDescending(u => u.Count));
        }

        [HttpGet]
        public async Task<IActionResult> SearchProduct(string term, int limit, int offset)
        {
            var searchResult =
                (from product in _context.Products
                where product.Name == term
                select new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Platform = product.Platform.ToString(),
                    DateCreated = product.DateCreated,
                    TotalRating = product.TotalRating
                }).ToList(); 
            return Ok(searchResult.Take(limit).Skip(offset));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
