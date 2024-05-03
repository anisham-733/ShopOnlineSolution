using ShopOnline.Models.Dtos;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services.Contracts
{
    public class ProductService : IProductService
    {
        //inject httpclient obj here
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                //where the collection of resources that we want to retrieve from webapi can be found.
                //json data will be auto converted to ienum<proddto>

                var products = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
