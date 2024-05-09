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
        public IShoppingCartService ShoppingCartService { get; set; }
        //firstly allproducts razor compo is rendered and we wnat user to see how many items are stroed in users shopping cart


        [Inject]
        public IEnumerable<ProductDto> ProductsList { get; set; }

        public string ErrorMessage { get; set; }

        //override a methid named OnInitializedAsync()

        //This method is associated with blazor lifecycle event
        protected override async Task OnInitializedAsync()
        {
            try
            {
                //retrieve products data from webapi
                ProductsList = await ProductService.GetItems();

                //fetch items stored in users shopping cart
                var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                //cal number of items stored in cart
                var totalQty = shoppingCartItems.Sum(i=>i.Qty);

                //now we send the items stored to any subscriber
                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
                //this will result in apt quantity value being displayed in the user header section of layout of our application
                //we want our event to be raised each time the quantity of items stored in cart changes

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
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
