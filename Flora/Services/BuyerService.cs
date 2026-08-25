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
        //FloraContext DI
        private readonly FloraContext _context;
        public BuyerService(FloraContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateAsync(CreateBuyerDto dto)
        {
            //چک کردن شماره تلفن تکراری
            var phonenumber = dto.PhoneNumber;
            var checkphonenumber = await _context.Buyers.AnyAsync(p => p.PhoneNumber == phonenumber);

            if (checkphonenumber)
            {
                return false;
            }

            //ثبت کاربر(خریدار) و هش کردن رمز
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
            return true;
        }
    }
}
