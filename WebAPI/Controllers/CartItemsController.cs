using BusinessObjects.Cart;
using BusinessObjects.Common;
using DTOs.CartDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebAPI.Middlewares;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartItemsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request)
        {
            var item = await _cartService.AddCartItemAsync(request);
            return Ok(ApiResult<CartItem>.Success(item, "Item added to cart"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var success = await _cartService.RemoveCartItemAsync(id);
            if (!success)
                return NotFound(ApiResult<string>.Error(null, new KeyNotFoundException("Cart item not found")));
            return Ok(ApiResult<bool>.Success(true, "Item removed from cart"));
        }
    }
}
