using BusinessObjects.Coupons;
using DTOs.CouponDTOs.Request;
using Microsoft.EntityFrameworkCore;
using Repositories.WorkSeeds.Interfaces;
using Services.Interfaces;

namespace Services.Implementations
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UserCouponService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> AddCouponForUserAsync(AddUserCouponRequest request)
        {
            var userId = _currentUser.GetUserId();
            if (userId == null)
                return false;

            var couponRepo = _unitOfWork.GetRepository<Coupon, Guid>();
            var userCouponRepo = _unitOfWork.GetRepository<UserCoupon, Guid>();

            var coupon = await couponRepo.FirstOrDefaultAsync(c => c.Code == request.Code && c.Status == CouponStatus.Active && c.StartDate <= DateTime.UtcNow && c.EndDate >= DateTime.UtcNow);
            if (coupon == null)
                return false;

            bool exists = await userCouponRepo.AnyAsync(uc => uc.UserId == userId && uc.CouponId == coupon.Id);
            if (exists)
                return true; // already added

            var entity = new UserCoupon
            {
                UserId = userId.Value,
                CouponId = coupon.Id,
                FirstUsedAt = DateTime.UtcNow,
                LastUsedAt = DateTime.UtcNow
            };
            await userCouponRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
