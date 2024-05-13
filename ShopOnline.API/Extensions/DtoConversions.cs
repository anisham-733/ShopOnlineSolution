using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;
using System.Runtime.CompilerServices;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        //arg-convert product to productdto, 2nd arg is one that u want to join and return type is prod dto
        //converts a collection of products and coll of categories into a collect of objects of type productdto

        //optimized code at last
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }

        //why to create dto-to separate classes for dto and entity
        //dto class can evolve independently from entity classes
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from product in productCategories
                    select new ProductCategoryDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        IconCSS = product.IconCSS,
                    }).ToList();

        }

        //here 1 obj of type Product and 1 obj of tyoe ProdCat into 1 obj of tyoe ProductDto

        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name

            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, 
                                                            IEnumerable<Product> products )
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = cartItem.Qty,
                        CartId = cartItem.CartId,
                        TotalPrice = product.Price * cartItem.Qty,
                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem,
                                               Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId,
                TotalPrice = product.Price * cartItem.Qty,

            };
        }

    }
}
