using BusinessObjects.Coupons;

namespace Services.Interfaces
{
    public interface ICouponService
    {
        Task<Coupon> CreateAsync(Coupon coupon);
        Task<bool> DeleteAsync(Guid id);
    }
}
