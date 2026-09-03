using Flora.Data;
using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;
using Flora.Enums;
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



        public async Task<LoginSellerResultDto?> LoginAsync(LoginSellerDto dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(p => p.PhoneNumber == dto.PhoneNumber);

            if (seller == null)
            {
                return null;
            }

            var passwordV = Hasher.VerifyPassword(
                dto.Password,
                seller.PasswordHash,
                seller.PasswordSalt
            );

            if (!passwordV)
            {
                return null;
            }

            var token = _tokenService.CreateSellerToken(seller);

            return new LoginSellerResultDto
            {
                Token = token
            };
        }



        public async Task<GetSellerDto?> GetMeAsync(int sellerId)
        {
            var seller = await _context.Sellers
                .SingleOrDefaultAsync(p => p.Id == sellerId);

            if (seller == null)
            {
                return null;
            }

            return new GetSellerDto
            {
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                PhoneNumber = seller.PhoneNumber
            };
        }



        public async Task<UpdateSellerResultDto> UpdateMeAsync(
    int sellerId,
    UpdateSellerDto dto)
        {
            var seller = await _context.Sellers
                .SingleOrDefaultAsync(p => p.Id == sellerId);

            if (seller == null)
            {
                return new UpdateSellerResultDto
                {
                    Status = UpdateProfileStatus.NotFound
                };
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName) &&
                string.IsNullOrWhiteSpace(dto.LastName) &&
                string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return new UpdateSellerResultDto
                {
                    Status = UpdateProfileStatus.NoChanges
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.FirstName) &&
                dto.FirstName == seller.FirstName)
            {
                return new UpdateSellerResultDto
                {
                    Status = UpdateProfileStatus.SameFirstName
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.LastName) &&
                dto.LastName == seller.LastName)
            {
                return new UpdateSellerResultDto
                {
                    Status = UpdateProfileStatus.SameLastName
                };
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                if (dto.PhoneNumber == seller.PhoneNumber)
                {
                    return new UpdateSellerResultDto
                    {
                        Status = UpdateProfileStatus.SamePhoneNumber
                    };
                }

                var phoneExists = await _context.Sellers
                    .AnyAsync(p =>
                        p.PhoneNumber == dto.PhoneNumber &&
                        p.Id != sellerId);

                if (phoneExists)
                {
                    return new UpdateSellerResultDto
                    {
                        Status = UpdateProfileStatus.PhoneNumberExists
                    };
                }
            }

            bool phoneChanged = !string.IsNullOrWhiteSpace(dto.PhoneNumber);

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
            {
                seller.FirstName = dto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(dto.LastName))
            {
                seller.LastName = dto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                seller.PhoneNumber = dto.PhoneNumber;
            }

            await _context.SaveChangesAsync();

            string? newToken = null;

            if (phoneChanged)
            {
                newToken = _tokenService.CreateSellerToken(seller);
            }

            return new UpdateSellerResultDto
            {
                Status = UpdateProfileStatus.Success,

                Seller = new GetSellerDto
                {
                    FirstName = seller.FirstName,
                    LastName = seller.LastName,
                    PhoneNumber = seller.PhoneNumber
                },

                Token = newToken
            };
        }
    }
}
