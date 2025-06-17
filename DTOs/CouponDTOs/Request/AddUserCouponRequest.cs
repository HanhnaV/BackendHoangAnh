using System.ComponentModel.DataAnnotations;

namespace DTOs.CouponDTOs.Request
{
    public class AddUserCouponRequest
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
    }
}
