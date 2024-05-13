using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;

namespace ShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;
        public ProductRepository(ShopOnlineDbContext dbContext)
        {
            _context = dbContext;

        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await _context.ProductCategories.ToListAsync();
        }        

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            //to establish joins btw 2 entities through this lambda expression - Include
            var product = await _context.Products
                         .Include(c => c.ProductCategory)
                         //filter the product data based on relevant product id passed to single or default async ()
                         .SingleOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await _context.Products
                        .Include(c => c.ProductCategory).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await _context.Products
                                    .Include(c => c.ProductCategory)
                                    .Where(p=>p.CategoryId == id).ToListAsync();
            return products;
        }
    }
}
