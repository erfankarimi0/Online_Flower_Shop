using Flora.Data;
using Flora.DTOs.Buyer;
using Flora.Models;
using Flora.Services.Interfaces;
using Flora.Utils;
using Microsoft.EntityFrameworkCore;

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
            var phonenumber = dto.PhoneNumber;

            var checkphonenumber = await _context.Buyers
                .AnyAsync(p => p.PhoneNumber == phonenumber);

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
            // چک کردن شماره همراه
            var phonenumberV = dto.PhoneNumber;

            var buyerV = await _context.Buyers
                .FirstOrDefaultAsync(p => p.PhoneNumber == phonenumberV);

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

            // ساخت JWT
            var token = _tokenService.CreateToken(buyerV);

            return new LoginBuyerResultDto
            {
                Token = token
            };
        }
    }
}