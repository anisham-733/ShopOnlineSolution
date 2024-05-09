using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class ShoppingCart
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            //we can use built in js runtime to call js function
            await MakeUpdateQtyButtonVisible(id, true);

        }

        private void UpdateItemTotalprice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null) 
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x=> x.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();

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
            CartChanged();
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

        private void CartChanged()
        {
            //method to be invoked whenever the state of users shopping cart is changed
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await JSRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }


        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if(qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        Qty = qty,
                        CartItemId = id
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                    UpdateItemTotalprice(returnedUpdateItemDto);
                    CartChanged();
                    //await MakeUpdateQtyButtonVisible(id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                    if(item != null)
                    {
                        item.Qty = qty;
                        item.TotalPrice = item.Price;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
