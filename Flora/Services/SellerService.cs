using Flora.Data;
using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;
using Flora.Models;
using Flora.Services.Interfaces;
using Flora.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Flora.Services
{
    public class SellerService : ISellerService
    {
        // FloraContext DI
        private readonly FloraContext _context;

        // TokenService DI
        private readonly ITokenService _tokenService;

        public SellerService(
            FloraContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<CreateSellerResultDto?> CreateAsync(CreateSellerDto dto)
        {
            //چک اینکه بیش از 1  فروشنده وجود نداشته باشه
            var sellerExists = await _context.Sellers.AnyAsync();

            if (sellerExists)
            {
                return null;
            }
            var hashed = Hasher.HashPassword(dto.Password);
            var seller = new Seller
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashed.HashedPassword,
                PasswordSalt = hashed.SaltBase64
            };
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();

            // ساخت JWT برای کاربر تازه ثبت‌نام‌شده
            var token = _tokenService.CreateSellerToken(seller);

            return new CreateSellerResultDto
            {
                Token = token
            };

        }

    }
}
