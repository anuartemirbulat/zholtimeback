using DataContracts.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegionalRides.DataContracts.Orders;
using RegionalRides.Services.Interfaces;

namespace RegionalRides.Api.Controllers
{
    [Route("api/regional/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var resp = await _ordersService.Create(request);
            return Ok(ApiResponse.Success(resp));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get([FromQuery] int? sourceKatoId, [FromQuery] int? destinationKatoId,
            [FromQuery] DateTime? departureDate)
        {
            var orders = await _ordersService.Get(sourceKatoId, destinationKatoId, departureDate);
            return Ok(ApiResponse.Success(orders));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrdersByCurrentProfile()
        {
            var orders = await _ordersService.GetOrdersByCurrentProfile();
            return Ok(ApiResponse.Success(orders));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Cancel([FromQuery] int orderId)
        {
            var res = await _ordersService.Cancel(orderId);
            return Ok(ApiResponse.Success(res));
        }
    }
}
