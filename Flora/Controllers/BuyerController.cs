using Flora.DTOs.Buyer;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Flora.Enums;
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

            Response.Cookies.Append(
                "access_token",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                }
            );

            return Ok(new
            {
                message = "ثبت‌ نام با موفقیت انجام شد."
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

            Response.Cookies.Append(
                "access_token",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                }
            );

            return Ok(new
            {
                message = "ورود با موفقیت انجام شد."
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var buyerIdValue = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (!int.TryParse(buyerIdValue, out var buyerId))
            {
                return Unauthorized();
            }

            var result = await _buyerService.GetMeAsync(buyerId);
            if (result == null)
            {
                return NotFound(new
                {
                    message = "اطلاعات کاربر پیدا نشد."
                });
            }

            return Ok(result);
        }



        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe(UpdateBuyerDto dto)
        {
            var buyerIdValue = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (!int.TryParse(buyerIdValue, out var buyerId))
            {
                return Unauthorized();
            }

            var result = await _buyerService.UpdateMeAsync(buyerId, dto);

            switch (result.Status)
            {
                case UpdateBuyerStatus.BuyerNotFound:
                    return NotFound(new
                    {
                        message = "کاربر پیدا نشد."
                    });

                case UpdateBuyerStatus.NoChanges:
                    return BadRequest(new
                    {
                        message = "حداقل یکی از اطلاعات را برای ویرایش وارد کنید."
                    });

                case UpdateBuyerStatus.SameFirstName:
                    return BadRequest(new
                    {
                        message = "نام جدید با نام فعلی شما یکسان است."
                    });

                case UpdateBuyerStatus.SameLastName:
                    return BadRequest(new
                    {
                        message = "نام خانوادگی جدید با نام خانوادگی فعلی شما یکسان است."
                    });

                case UpdateBuyerStatus.SamePhoneNumber:
                    return BadRequest(new
                    {
                        message = "شماره تلفن جدید با شماره فعلی شما یکسان است."
                    });

                case UpdateBuyerStatus.PhoneNumberExists:
                    return Conflict(new
                    {
                        message = "این شماره تلفن قبلاً ثبت شده است."
                    });

                case UpdateBuyerStatus.Success:

                    // اگر شماره تلفن تغییر کرده باشد Service یک Token جدید ساخته است.
                    if (result.Token != null)
                    {
                        Response.Cookies.Append(
                            "access_token",
                            result.Token,
                            new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = DateTimeOffset.UtcNow.AddDays(7)
                            }
                        );
                    }

                    return Ok(new
                    {
                        message = "اطلاعات با موفقیت ویرایش شد.",
                        buyer = result.Buyer
                    });

                default:
                    return BadRequest();
            }
        }



        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new
            {
                message = "با موفقیت از حساب کاربری خارج شدید."
            });
        }



        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var buyerIdValue = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (!int.TryParse(buyerIdValue, out var buyerId))
            {
                return Unauthorized();
            }

            var result = await _buyerService.DeleteMeAsync(buyerId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "اطلاعات کاربر پیدا نشد."
                });
            }

            Response.Cookies.Delete("access_token");

            return Ok(new
            {
                message = "حساب کاربری با موفقیت حذف شد."
            });
        }
    }

}