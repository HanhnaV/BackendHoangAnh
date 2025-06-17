using BusinessObjects.Cart;
using DTOs.CartDTOs;

namespace Services.Interfaces
{
    public interface ICartService
    {
        Task<CartItem> AddCartItemAsync(AddCartItemRequest request);
        Task<bool> RemoveCartItemAsync(Guid cartItemId);
    }
}
