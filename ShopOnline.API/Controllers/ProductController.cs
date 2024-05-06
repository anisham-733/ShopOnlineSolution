using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }
        //actionresult-returns requested data as well as response status code
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            //write try and press tab key twice
            try
            {
                //could be optimized using Include in Linq query
                var products = await _productRepository.GetItems();
                var productCategories = await _productRepository.GetCategories();

                if (products == null || productCategories == null) { return NotFound(); }
                else
                {
                    //prods-1st arg and cat-2nd arg
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);
                }

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("{id:int}")]
        //return one object of type ProductDto
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            //write try and press tab key twice
            try
            {
                //could be optimized using Include in Linq query
                var product = await _productRepository.GetItem(id);

                if (product == null) { return BadRequest(); }
                else
                {
                    var productCategory = await _productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productCategory);
                    return Ok(productDto);
                }

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

    }
}
