using BusinessObjects.Cart;
using BusinessObjects.Products;
using DTOs.CartDTOs;
using Repositories.Interfaces;
using Repositories.WorkSeeds.Interfaces;
using Services.Commons;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CartService : BaseService<CartItem, Guid>, ICartService
    {
        public CartService(
            IGenericRepository<CartItem, Guid> repository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ICurrentTime currentTime)
            : base(repository, currentUserService, unitOfWork, currentTime)
        {
        }

        public async Task<CartItem> AddCartItemAsync(AddCartItemRequest request)
        {
            var productRepo = _unitOfWork.GetRepository<Product, Guid>();
            var variantRepo = _unitOfWork.GetRepository<ProductVariant, Guid>();

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            decimal unitPrice = product.SalePrice ?? product.Price;

            if (request.ProductVariantId.HasValue)
            {
                var variant = await variantRepo.GetByIdAsync(request.ProductVariantId.Value);
                if (variant == null || variant.ProductId != product.Id)
                    throw new KeyNotFoundException("Product variant not found");

                if (variant.PriceAdjustment.HasValue)
                    unitPrice += variant.PriceAdjustment.Value;

                request.SelectedColor ??= variant.Color;
                request.SelectedSize ??= variant.Size;
            }

            var entity = new CartItem
            {
                UserId = _currentUserService.GetUserId(),
                ProductId = request.ProductId,
                ProductVariantId = request.ProductVariantId,
                SelectedColor = request.SelectedColor,
                SelectedSize = request.SelectedSize,
                Quantity = request.Quantity,
                UnitPrice = unitPrice,
                CreatedAt = _currentTime.GetVietnamTime(),
                UpdatedAt = _currentTime.GetVietnamTime()
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> RemoveCartItemAsync(Guid cartItemId)
        {
            var result = await _repository.DeleteAsync(cartItemId);
            if (result)
                await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
