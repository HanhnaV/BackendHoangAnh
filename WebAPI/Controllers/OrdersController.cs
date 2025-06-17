using BusinessObjects.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.WorkSeeds.Interfaces;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public OrdersController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var repo = _unitOfWork.GetRepository<Order, Guid>();
            var query = repo.GetQueryable().Include(o => o.OrderItems);

            if (!User.IsInRole("ADMIN") && !User.IsInRole("ORDER_PROCESSOR"))
            {
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                    return Unauthorized();
                query = query.Where(o => o.UserId == userId);
            }

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
            return Ok(orders);
        }

        // PUT: api/Orders/{id}/confirm
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "ADMIN,ORDER_PROCESSOR")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await repo.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            if (order.Status != OrderStatus.Pending)
                return BadRequest("Order cannot be confirmed in its current status.");

            order.Status = OrderStatus.Confirmed;
            await repo.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
