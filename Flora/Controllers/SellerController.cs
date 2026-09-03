using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;
using Flora.Enums;
using Flora.Services;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Flora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSellerDto dto)
        {
            var result = await _sellerService.CreateAsync(dto);
            if (result == null)
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



        [HttpGet("me")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Me()
        {
            var sellerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (sellerIdClaim == null)
            {
                return Unauthorized();
            }

            var sellerId = int.Parse(sellerIdClaim.Value);

            var seller = await _sellerService.GetMeAsync(sellerId);

            if (seller == null)
            {
                return NotFound(new
                {
                    message = "اطلاعات فروشنده پیدا نشد."
                });
            }

            return Ok(seller);
        }



        [HttpPut("me")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateMe(UpdateSellerDto dto)
        {
            var sellerIdValue = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            if (!int.TryParse(sellerIdValue, out var sellerId))
            {
                return Unauthorized();
            }

            var result = await _sellerService.UpdateMeAsync(sellerId, dto);

            switch (result.Status)
            {
                case UpdateProfileStatus.NotFound:
                    return NotFound(new
                    {
                        message = "فروشنده پیدا نشد."
                    });

                case UpdateProfileStatus.NoChanges:
                    return BadRequest(new
                    {
                        message = "حداقل یکی از اطلاعات را برای ویرایش وارد کنید."
                    });

                case UpdateProfileStatus.SameFirstName:
                    return BadRequest(new
                    {
                        message = "نام جدید با نام فعلی شما یکسان است."
                    });

                case UpdateProfileStatus.SameLastName:
                    return BadRequest(new
                    {
                        message = "نام خانوادگی جدید با نام خانوادگی فعلی شما یکسان است."
                    });

                case UpdateProfileStatus.SamePhoneNumber:
                    return BadRequest(new
                    {
                        message = "شماره تلفن جدید با شماره فعلی شما یکسان است."
                    });

                case UpdateProfileStatus.PhoneNumberExists:
                    return Conflict(new
                    {
                        message = "این شماره تلفن قبلاً ثبت شده است."
                    });

                case UpdateProfileStatus.Success:

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
                        seller = result.Seller
                    });

                default:
                    return BadRequest();
            }
        }
    }
}
