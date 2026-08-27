using Flora.DTOs.Buyer;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBuyerDto dto)
        {
            var result = await _buyerService.CreateAsync(dto);

            if (result == null)
            {
                return Conflict(new
                {
                    message = "این شماره تلفن قبلاً ثبت شده است. لطفاً وارد شوید."
                });
            }

            return Ok(new
            {
                message = "ثبت‌ نام با موفقیت انجام شد.",
                token = result.Token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginBuyerDto dto)
        {
            var result = await _buyerService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "شماره تلفن یا رمز عبور اشتباه است."
                });
            }

            return Ok(new
            {
                message = "ورود با موفقیت انجام شد.",
                token = result.Token
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                message = "شما احراز هویت شدید.",
                buyerId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )?.Value
            });
        }
    }
}

