using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class Checkout
    {
        [Inject]
        //in order to call initPayPal, we need this
        //javascript interoperability feature
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }

        protected decimal PaymentAmount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //optimize code and improve performance by local storage in order to avoid so many calls to the server
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                if(ShoppingCartItems != null)
                {
                    //using guid to uniquely identify a particular order
                    Guid orderGuid = Guid.NewGuid();
                    PaymentAmount = ShoppingCartItems.Sum(x => x.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(x => x.Qty);
                    PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //this method is called in response to the occurrence of blazor lifecycle event
         //that occurs after the razor compo is rendered
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //firstRender - only fired once after our razor compo is rendered
            try
            {
                if(firstRender)
                {
                    //call initpaypal
                    await JSRuntime.InvokeVoidAsync("initPayPalButton");
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        //generate client id where we can link our paypal acc to app
        //we will use sandbox feature to implement the server side part of integration functionality, so that we can integrate paypal into our app


    }
}
