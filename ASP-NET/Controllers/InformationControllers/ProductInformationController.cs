using AutoMapper;
using Business.Interfaces;
using Business.Repositories;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{
    [ApiController]
    [Route("api/games")]
    [Produces("application/json")]
    public class ProductInformationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductInformationController(IMapper mapper, ApplicationDbContext context, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
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
            var result = await _productService.GetTopPlatformsAsync(3);
            if (result == null)
                return BadRequest("There are no products in the database");
            return Ok(result);
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
            var result = await _productService.SearchProductByNameAsync(term, limit, offset);
            if (result == null)
                return BadRequest("No items were located");
            return Ok(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() => View();
    }
}
