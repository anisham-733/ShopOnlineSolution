using Microsoft.AspNetCore.Components;

namespace ShopOnline.Web.Pages
{
    public partial class DisplayError
    {
        [Parameter]
        public string ErrorMessage { get; set; }
    }
}
