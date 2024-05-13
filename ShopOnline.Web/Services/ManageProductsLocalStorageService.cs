using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IProductService productService;

        private const string key = "ProductCollection";

        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            this.localStorageService = localStorageService;
            this.productService = productService;
        }


        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            // to retrieve relevant data from local storage
            return await this.localStorageService.GetItemAsync<IEnumerable<ProductDto>>(key)
                //if key is null, retrieve relevant data from server
                    ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }

        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await this.productService.GetItems();
            if(productCollection != null)
            {
                await this.localStorageService.SetItemAsync(key, productCollection);
            }
            return productCollection;
        }
    }
}
