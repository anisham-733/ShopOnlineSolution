using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;
using System.Runtime.CompilerServices;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        //arg-convert product to productdto, 2nd arg is one that u want to join and return type is prod dto
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories )
        {
            return (from product in products
                    join category in productCategories
                    on product.CategoryId equals category.Id
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.CategoryId,
                        CategoryName = category.Name
                    }).ToList();
        }
    }
}
