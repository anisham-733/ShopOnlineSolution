using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopOnline.Web;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//we will use httpclient to call relevant action methods within webapi, 
//added base url of webapi project

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7206/") });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IManageCartItemsLocalStorageService, ManageCartItemsLocalStorageService>();
builder.Services.AddScoped<IManageProductsLocalStorageService, ManageProductsLocalStorageService>();

await builder.Build().RunAsync();
