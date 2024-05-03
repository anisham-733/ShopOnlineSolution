using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Pages
{
    public partial class DisplayProducts
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }


    }
}
