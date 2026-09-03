using Flora.DTOs.Auth;
using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;
using Flora.Enums;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IBuyerService _buyerService;
        private readonly ISellerService _sellerService;

        public AuthController(
            IBuyerService buyerService,
            ISellerService sellerService)
        {
            _buyerService = buyerService;
            _sellerService = sellerService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Login Buyer
            if (dto.Role == UserRole.Buyer)
            {
                var result = await _buyerService.LoginAsync(
                    new LoginBuyerDto
                    {
                        PhoneNumber = dto.PhoneNumber,
                        Password = dto.Password
                    });

                if (result == null)
                {
                    return Unauthorized(new
                    {
                        message = "شماره تلفن یا رمز عبور اشتباه است."
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
                    message = "ورود با موفقیت انجام شد."
                });
            }

            // Login Seller
            if (dto.Role == UserRole.Seller)
            {
                var result = await _sellerService.LoginAsync(
                    new LoginSellerDto
                    {
                        PhoneNumber = dto.PhoneNumber,
                        Password = dto.Password
                    });

                if (result == null)
                {
                    return Unauthorized(new
                    {
                        message = "شماره تلفن یا رمز عبور اشتباه است."
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
                    message = "ورود با موفقیت انجام شد."
                });
            }

            return BadRequest(new
            {
                message = "نوع حساب نامعتبر است."
            });
        }



        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("access_token"))
            {
                Response.Cookies.Delete("access_token");
            }

            return Ok(new
            {
                message = "با موفقیت از حساب کاربری خارج شدید."
            });
        }
    }
}