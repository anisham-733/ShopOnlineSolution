using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
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

                var httpResponse = await _httpClient.GetAsync("api/Product");
                if (httpResponse.IsSuccessStatusCode)
                {
                    //Scenario - no data returned from server but req was successfull

                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    return await httpResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await httpResponse.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                //convert json into apt objects
                //GetAsync doesnt convert the server side json data to apt objects
                var httpResponse = await _httpClient.GetAsync($"api/Product/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    //Scenario - no data returned from server but req was successfull

                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }
                    return await httpResponse.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await httpResponse.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Product/GetProductCategories");
                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code - {response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //display the products on UI filtered by category id
        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code - {response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
