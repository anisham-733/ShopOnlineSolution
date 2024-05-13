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
                ////// ** OPTIMIZATIONS ** //////////////
                //could be optimized using Include in Linq query
                // 2 separate calls to database/server layer
                //optimize it by making 1 query to the database

                //Use local storage(i.e data stored on client within users browser) in blazor component for both product data and shopping cart data
                var products = await _productRepository.GetItems();

                if (products == null) { return NotFound(); }
                else
                {
                    //prods-1st arg and cat-2nd arg
                    var productDtos = products.ConvertToDto();
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
                //GetItem() - linq query jo product repo pe likhi is responsible for including and giving us prod cat id and name too
                var product = await _productRepository.GetItem(id);

                if (product == null) { return BadRequest(); }
                else
                {
                    var productDto = product.ConvertToDto();
                    return Ok(productDto);
                }

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetCategories();
                var productCategoryDto = productCategories.ConvertToDto();
                return Ok(productCategoryDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetItemsByCategory(categoryId);
                var productDtos = products.ConvertToDto();
                return Ok(productDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }

    }
}
