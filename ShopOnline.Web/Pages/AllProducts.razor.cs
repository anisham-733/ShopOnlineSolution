using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class AllProducts
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IEnumerable<ProductDto> ProductsList { get; set; }

        //override a methid named OnInitializedAsync()

        //This method is associated with blazor lifecycle event
        protected override async Task OnInitializedAsync()
        {
            //retrieve products data from webapi
            ProductsList = await ProductService.GetItems();
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in ProductsList
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDto)
        {
            return groupedProductDto.FirstOrDefault(pg => pg.CategoryId == groupedProductDto.Key).CategoryName;
        }
    }
}
