using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;
using Flora.Services;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _SellerService;

        public SellerController(ISellerService sellerService)
        {
            _SellerService = sellerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSellerDto dto)
        {
            var result = await _SellerService.CreateAsync(dto);
            if(result == null)
            {
                return Conflict(new
                {
                    message = "قبلا یک فروشنده ثبت نام کرده است."
                });
            }
            Response.Cookies.Append(
            "access_token",
            result.Token,
            new CookieOptions
            {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                message = "ثبت‌ نام با موفقیت انجام شد."
            });

        }
    }
}
