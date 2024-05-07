using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class ShoppingCart
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            //now i want the razor component to render again after del operation is successfull.
            //instead of giving one more server backend call, to render the component again
            //we wil use a method to remove items from the client side shopping cart items collection
            //and do this in a separate method

            //Better Performance solution 
            RemoveCartItem(id);

        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            //shopping cart items client side is of type ienum, and no easy way to remove item from this collection
            //one solution is to change type from ienum to list
            ShoppingCartItems.Remove(cartItemDto);


        }
    }
}
