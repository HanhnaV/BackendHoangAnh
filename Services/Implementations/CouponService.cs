using BusinessObjects.Coupons;
using Repositories.Interfaces;
using Repositories.WorkSeeds.Interfaces;
using Services.Commons;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CouponService : BaseService<Coupon, Guid>, ICouponService
    {
        public CouponService(
            IGenericRepository<Coupon, Guid> repository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ICurrentTime currentTime)
            : base(repository, currentUserService, unitOfWork, currentTime)
        {
        }
    }
}
