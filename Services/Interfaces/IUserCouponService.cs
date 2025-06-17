using DTOs.CouponDTOs.Request;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserCouponService
    {
        Task<bool> AddCouponForUserAsync(AddUserCouponRequest request);
    }
}
