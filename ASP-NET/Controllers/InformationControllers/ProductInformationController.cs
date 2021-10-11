using ASP_NET.Models;
using AutoMapper;
using Business.Interfaces;
using Business.Repositories;
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
        private readonly IMapper _mapper;
        private IRepository<Product> ProductRep;

        public ProductInformationController(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            ProductRep = new ProductRepository(context);
        }

        [HttpGet("top-platforms")]
        [SwaggerOperation(
            Summary = "Finds top 3 platforms",
            Description = "Groups all products by platform and takes top 3",
            OperationId = "FindTopPlatforms",
            Tags = new[] { "Search", "Product" })]
        [SwaggerResponse(200, "Returned top 3 platforms", typeof(IList<Product>))]
        [SwaggerResponse(400, "No products were found")]
        public IActionResult FindTopPlatforms()
        {
            if (ProductRep.GetList() == null)
                return BadRequest("There are no products in the database");
            return Ok(ProductRep.GetList().GroupBy(u => u.Platform).Select(u => new { Platform = u.Key.ToString(), Count = u.Count() }).OrderByDescending(u => u.Count).Take(3).ToList());
        }

        [HttpGet("")]
        [SwaggerOperation(
            Summary = "Search",
            Description = "Searches and offsets limited amount of products specified by term from all products",
            OperationId = "SearchProduct",
            Tags = new[] { "Search", "Product" })]
        [SwaggerResponse(200, "Returned a list of products by specified term", typeof(IList<Product>))]
        [SwaggerResponse(400, "No products were found")]
        public IActionResult SearchProduct([SwaggerParameter("Term used to search through Product table", Required = true)] string term, 
                                                       [SwaggerParameter("Amount of items to return", Required = true)] int limit,
                                                       [SwaggerParameter("Amount of items to skip", Required = true)] int offset)
        {
            if (ProductRep.GetList() == null)
                return BadRequest("No items were located");
            var searchResult = ProductRep.GetList().Where(u=>u.Name == term).Select(u => new 
            {
                Id = u.Id,
                Name = u.Name,
                Platform = u.Platform.ToString(),
                DateCreated = u.DateCreated,
                TotalRating = u.TotalRating
            })
                .Skip(offset).Take(limit).ToList();
            return Ok(searchResult);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
