using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class ProductsByCategory
    {
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public string CategoryName { get; set; }

        public string ErrorMessage { get; set; }

        //want to fetch the products only when category id parameter is sent
        //hence this method called only after the parameter value is set
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await GetProductCollectionByCategoryId(CategoryId);
                if(Products != null && Products.Count()  > 0)
                {
                    var productDto = Products.FirstOrDefault(p=>p.CategoryId == CategoryId);
                    if(productDto != null)
                    {
                        CategoryName = productDto.CategoryName;

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ManageProductsLocalStorageService.GetCollection();
            if(productCollection != null)
            {
                return productCollection.Where(p=>p.CategoryId == categoryId);

            }
            else
            {
                return await ProductService.GetItemsByCategory(categoryId);
            }
        }
    }
}
