using Flora.Data;
using Flora.DTOs.Buyer;
using Flora.Enums;
using Flora.Models;
using Flora.Services.Interfaces;
using Flora.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Flora.Services
{
    public class BuyerService : IBuyerService
    {
        // FloraContext DI
        private readonly FloraContext _context;

        // TokenService DI
        private readonly ITokenService _tokenService;

        public BuyerService(
            FloraContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        public async Task<CreateBuyerResultDto?> CreateAsync(CreateBuyerDto dto)
        {
            // چک کردن شماره تلفن تکراری
            var checkphonenumber = await _context.Buyers
                .AnyAsync(p => p.PhoneNumber == dto.PhoneNumber && p.Status != BuyerStatus.Deleted.ToString());

            if (checkphonenumber)
            {
                return null;
            }

            // ثبت کاربر و هش کردن رمز
            var hashed = Hasher.HashPassword(dto.Password);
            var buyer = new Buyer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashed.HashedPassword,
                PasswordSalt = hashed.SaltBase64
            };

            _context.Buyers.Add(buyer);

            await _context.SaveChangesAsync();

            // ساخت JWT برای کاربر تازه ثبت‌نام‌شده
            var token = _tokenService.CreateToken(buyer);

            return new CreateBuyerResultDto
            {
                Token = token
            };
        }

        public async Task<LoginBuyerResultDto?> LoginAsync(LoginBuyerDto dto)
        {
            // چک کردن شماره همراه و وضعیت حساب
            var buyerV = await _context.Buyers
                .FirstOrDefaultAsync(p =>
                    p.PhoneNumber == dto.PhoneNumber &&
                    p.Status != BuyerStatus.Deleted.ToString() &&
                    p.Status != BuyerStatus.Blocked.ToString());

            if (buyerV == null)
            {
                return null;
            }

            // بررسی رمز عبور
            var PasswordV = Hasher.VerifyPassword(
                dto.Password,
                buyerV.PasswordHash,
                buyerV.PasswordSalt
            );

            if (!PasswordV)
            {
                return null;
            }
            // اضافه کردن بخش آخرین لاگین جهت غیرفعال کردن وضعیت خریدار
            buyerV.LastLoginDate = DateTime.UtcNow;

            if (buyerV.Status == BuyerStatus.Inactive.ToString())
            {
                buyerV.Status = BuyerStatus.Active.ToString();
            }

            await _context.SaveChangesAsync();


            // ساخت JWT
            var token = _tokenService.CreateToken(buyerV);

            return new LoginBuyerResultDto
            {
                Token = token
            };
        }


        public async Task<GetBuyerDto?> GetMeAsync(int buyerId)
        {
            var buyer = await _context.Buyers
                .SingleOrDefaultAsync(p => p.Id == buyerId);

            if (buyer == null)
            {
                return null;
            }

            return new GetBuyerDto
            {
                FirstName = buyer.FirstName,
                LastName = buyer.LastName,
                PhoneNumber = buyer.PhoneNumber
            };
        }


        public async Task<UpdateBuyerResultDto> UpdateMeAsync(int buyerId,UpdateBuyerDto dto)
        {
            var buyer = await _context.Buyers
                .SingleOrDefaultAsync(p => p.Id == buyerId);

            if (buyer == null)
            {
                return new UpdateBuyerResultDto
                {
                    Status = UpdateProfileStatus.NotFound
                };
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName) &&
                string.IsNullOrWhiteSpace(dto.LastName) &&
                string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return new UpdateBuyerResultDto
                {
                    Status = UpdateProfileStatus.NoChanges
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.FirstName) &&
                dto.FirstName == buyer.FirstName)
            {
                return new UpdateBuyerResultDto
                {
                    Status = UpdateProfileStatus.SameFirstName
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.LastName) &&
                dto.LastName == buyer.LastName)
            {
                return new UpdateBuyerResultDto
                {
                    Status = UpdateProfileStatus.SameLastName
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                if (dto.PhoneNumber == buyer.PhoneNumber)
                {
                    return new UpdateBuyerResultDto
                    {
                        Status = UpdateProfileStatus.SamePhoneNumber
                    };
                }

                var phoneExists = await _context.Buyers
                    .AnyAsync(p =>
                        p.PhoneNumber == dto.PhoneNumber &&
                        p.Id != buyerId &&
                        p.Status != BuyerStatus.Deleted.ToString());

                if (phoneExists)
                {
                    return new UpdateBuyerResultDto
                    {
                        Status = UpdateProfileStatus.PhoneNumberExists
                    };
                }
            }

            bool phoneChanged = !string.IsNullOrWhiteSpace(dto.PhoneNumber);
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
            {
                buyer.FirstName = dto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(dto.LastName))
            {
                buyer.LastName = dto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                buyer.PhoneNumber = dto.PhoneNumber;
            }

            await _context.SaveChangesAsync();

            string? newToken = null;

            if (phoneChanged)
            {
                newToken = _tokenService.CreateToken(buyer);
            }

            return new UpdateBuyerResultDto
            {
                Status = UpdateProfileStatus.Success,

                Buyer = new GetBuyerDto
                {
                    FirstName = buyer.FirstName,
                    LastName = buyer.LastName,
                    PhoneNumber = buyer.PhoneNumber
                },

                Token = newToken
            };
        }



        public async Task<bool> DeleteMeAsync(int buyerId)
        {
            var buyer = await _context.Buyers
                .SingleOrDefaultAsync(p => p.Id == buyerId);

            if (buyer == null)
            {
                return false;
            }

            buyer.Status = BuyerStatus.Deleted.ToString();

            await _context.SaveChangesAsync();

            return true;
        }
    }
}