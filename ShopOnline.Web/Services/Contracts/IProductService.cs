﻿using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        //to get items from the method(controller) that resides in webapi component
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);

        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
        Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId);
    }
}
