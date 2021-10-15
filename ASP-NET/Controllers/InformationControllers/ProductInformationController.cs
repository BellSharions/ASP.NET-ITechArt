using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{
    [ApiController]
    [Route("api/games")]
    [Produces("application/json")]
    public class ProductInformationController : Controller
    {
        private readonly IProductService _productService;

        public ProductInformationController(IProductService productService)
        {
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

        [HttpGet("id/{id}")]
        [SwaggerOperation(
            Summary = "Find product info",
            Description = "Uses specified product id to find it in the database",
            OperationId = "FindProductInfo",
            Tags = new[] { "Search", "Product" })]
        [SwaggerResponse(200, "Returned product info", typeof(ProductInfoDto))]
        [SwaggerResponse(400, "No products were found")]
        public async Task<IActionResult> FindProductInfo(int id)
        {
            var result = await _productService.GetProductInfoByIdAsync(id);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        [SwaggerOperation(
            Summary = "Create product",
            Description = "Creates new product, using given information",
            OperationId = "CreateProduct",
            Tags = new[] { "Product", "Information" })]
        [SwaggerResponse(200, "Product was created")]
        [SwaggerResponse(400, "Product was not created")]
        public async Task<IActionResult> CreateProduct([FromForm, SwaggerParameter("Information to create product", Required = true)] ProductCreationDto info)
        {
            var result = await _productService.CreateProductAsync(info);
            if (result.Type == ResultType.BadRequest)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        [SwaggerOperation(
            Summary = "Change product information",
            Description = "Searches specified product and changes information taken from body",
            OperationId = "ChangeProductInfo",
            Tags = new[] { "Information", "Product" })]
        [SwaggerResponse(200, "Product information was updated")]
        [SwaggerResponse(400, "Product information was not updated")]
        public async Task<IActionResult> ChangeProductInfo([FromForm, SwaggerParameter("Modified user information", Required = true)] ProductChangeDto info)
        {
            var result = await _productService.ChangeProductInfoAsync(info);
            if (result.Type == ResultType.BadRequest)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id/{id}")]
        [SwaggerOperation(
            Summary = "Delete product",
            Description = "Searches specified product by id and deletes it",
            OperationId = "DeleteProduct",
            Tags = new[] { "Information", "Product" })]
        [SwaggerResponse(200, "Product was deleted")]
        [SwaggerResponse(400, "Product was not deleted")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (result.Type == ResultType.BadRequest)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() => View();
    }
}
