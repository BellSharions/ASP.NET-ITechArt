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
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{
    [ApiController]
    [Route("api/games")]
    [Produces("application/json")]
    public class ProductInformationController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductInformationController(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("top-platforms")]
        [SwaggerOperation(
            Summary = "Finds top 3 platforms",
            Description = "Groups all products by platform and takes top 3",
            OperationId = "FindTopPlatforms",
            Tags = new[] { "Search", "Product" })]
        [SwaggerResponse(200, "Returned top 3 platforms", typeof(IList<Product>))]
        [SwaggerResponse(400, "No products were found")]
        public async Task<IActionResult> FindTopPlatforms()
        {
            if (_context.Products != null)
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
            else
                return BadRequest("There are no products in the database");
        }

        [HttpGet("")]
        [SwaggerOperation(
            Summary = "Search",
            Description = "Searches and offsets limited amount of products specified by term from all products",
            OperationId = "SearchProduct",
            Tags = new[] { "Search", "Product" })]
        [SwaggerResponse(200, "Returned a list of products by specified term", typeof(IList<Product>))]
        [SwaggerResponse(400, "No products were found")]
        public async Task<IActionResult> SearchProduct([SwaggerParameter("Term used to search through Product table", Required = true)] string term, 
                                                       [SwaggerParameter("Amount of items to return", Required = true)] int limit,
                                                       [SwaggerParameter("Amount of items to skip", Required = true)] int offset)
        {
            if (_context.Products != null)
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
            else
                return BadRequest("Please specify the term in search");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
