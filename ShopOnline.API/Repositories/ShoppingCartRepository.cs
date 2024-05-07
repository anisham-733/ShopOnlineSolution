using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _context;
        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _context = shopOnlineDbContext;            
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _context.CartItems.AnyAsync(c=>c.CartId == cartId
                                                            && c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if(await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in  _context.Products
                                    where product.Id == cartItemToAddDto.ProductId
                                    select new CartItem
                                    {
                                        CartId = cartItemToAddDto.CartId,
                                        ProductId = product.Id,
                                        Qty = cartItemToAddDto.Qty,
                                    }
                                  ).SingleOrDefaultAsync();

                if(item!=null)
                {
                    var result = await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            //id-entity that we wish to delete
            CartItem? item = await _context.CartItems.FindAsync(id);
            if(item!= null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return item;
        }

        //item stored in shopping cart
        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _context.Carts
                          join cartItem in _context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId,
                          }).SingleAsync();
        }

        //products stored in shopping cart
        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in  _context.Carts
                          join cartItem in _context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId= cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId,
                          }).ToListAsync();
        }

        public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
