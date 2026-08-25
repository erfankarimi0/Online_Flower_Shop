using Flora.DTOs.Buyer;
using Flora.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
            if (result == false)
            {
                return Conflict(new
                {
                    messagePN = "این شماره تلفن قبلاً ثبت شده است."
                });
             }
            else
            {
                return Ok();
            }
        }
    }

}
