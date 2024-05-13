using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services;
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

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        
        public string ErrorMessage { get; set; }

        //override a methid named OnInitializedAsync()

        //This method is associated with blazor lifecycle event
        protected override async Task OnInitializedAsync()
        {
            try
            {
                //here we want to clear relevant local storage items from local storage
                //and force the server to set it
                await ClearLocalStorage();

                //retrieve products data from webapi
                //retrieve relevant data from server and save the data to local storage
                //trips to server saved and performance of app is potentially enhanced
                ProductsList = await ManageProductsLocalStorageService.GetCollection();

                //fetch items stored in users shopping cart
                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
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

        private async Task ClearLocalStorage()
        {
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
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
