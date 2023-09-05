using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using System.Xml.Linq;
using ShopOnline.Api.Views;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository productRepository;

    public ProductController(IProductRepository productRepository) => this.productRepository = productRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
    {
        try
        {
            var products = await this.productRepository.GetItems();

            if (products == null) return NotFound();
            else
            {
                var productDtos = products.ConvertToDto();
                return Ok(productDtos);
            }

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
               
        }
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetItem(int id)
    {
        try
        {
            var product = await this.productRepository.GetItem(id);
               
            if (product == null)
            {
                return BadRequest();
            }
            else
            {
                    
                var productDto = product.ConvertToDto();

                return Ok(productDto);
            }

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");

        }
    }

    [HttpGet]
    [Route(nameof(GetProductCategories))]
    public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
    {
        try
        {
            var productCategories = await productRepository.GetCategories();
                
            var productCategoryDtos = productCategories.ConvertToDto();

            return Ok(productCategoryDtos);

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }

    }

    [HttpGet]
    [Route("{categoryId}/GetItemsByCategory")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
    {
        try
        {
            var products = await productRepository.GetItemsByCategory(categoryId);

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);
            
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductView productDto)
    {
        var product = await productRepository.AddProduct(productDto); 

        if (product == null) return BadRequest("Xatolik yuz berdi.");

        return Ok(product);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateProduct(string name, [FromForm] ProductView productDto)
    {
        var updatedProduct = await productRepository.UpdateProduct(name, productDto);

        if (updatedProduct == null)
        {
            return NotFound();
        }

        return Ok(updatedProduct);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(string name)
    {
        await productRepository.DeleteProduct(name); 
 

        return Ok("Product is deleted");
    }

    [HttpGet("ProductsList")]
    public async Task<IActionResult> GetAllProductsList() => Ok(await productRepository.GetAll());
}