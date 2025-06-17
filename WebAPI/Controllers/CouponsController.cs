using BusinessObjects.Coupons;
using DTOs.CouponDTOs.Request;
using DTOs.CouponDTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CouponResponse>> CreateCoupon([FromBody] CreateCouponRequest request)
        {
            var coupon = new Coupon
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Value = request.Value,
                MinOrderAmount = request.MinOrderAmount,
                MaxDiscountAmount = request.MaxDiscountAmount,
                UsageLimit = request.UsageLimit,
                UsageLimitPerUser = request.UsageLimitPerUser,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = CouponStatus.Active,
            };

            var created = await _couponService.CreateAsync(coupon);

            var response = new CouponResponse
            {
                Id = created.Id,
                Code = created.Code,
                Name = created.Name,
                Description = created.Description,
                Type = created.Type,
                Value = created.Value,
                MinOrderAmount = created.MinOrderAmount,
                MaxDiscountAmount = created.MaxDiscountAmount,
                UsageLimit = created.UsageLimit,
                UsedCount = created.UsedCount,
                UsageLimitPerUser = created.UsageLimitPerUser,
                StartDate = created.StartDate,
                EndDate = created.EndDate,
                Status = created.Status,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoupon(Guid id)
        {
            var result = await _couponService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
