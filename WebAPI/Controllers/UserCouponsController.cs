using DTOs.CouponDTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCouponsController : ControllerBase
    {
        private readonly IUserCouponService _service;

        public UserCouponsController(IUserCouponService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCoupon([FromBody] AddUserCouponRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddCouponForUserAsync(request);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
