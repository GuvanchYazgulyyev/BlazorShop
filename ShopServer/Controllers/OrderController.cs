using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopServer.Services.Infrastuce;
using ShopServer.Services.Services;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrderController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        /// <summary>
        ///  Kullancıları Getir.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Orders")]
        public async Task<ServiceResponse<List<OrderDTO>>> GetOrder()
        {
            return new ServiceResponse<List<OrderDTO>>()
            {
                Value = await _ordersService.GetOrder()
            };
        }
        ///
        // Create Order
        [HttpPost("Create")]
        public async Task<ServiceResponse<OrderDTO>> CreateOrder([FromBody] OrderDTO Order)
        {
            return new ServiceResponse<OrderDTO>()
            {
                Value = await _ordersService.CreateOrder(Order)
            };
        }
        ///Order Update
        ///
        [HttpPut("Update")]
        public async Task<ServiceResponse<OrderDTO>> UpdateOrder([FromBody] OrderDTO Order)
        {
            return new ServiceResponse<OrderDTO>()
            {
                Value = await _ordersService.UpdateOrder(Order)
            };
        }

        /// Id Ye Göre Getir 
        [HttpPost("OrderById/{Id}")]
        public async Task<ServiceResponse<OrderDTO>> GetOrderById(Guid Id)
        {
            return new ServiceResponse<OrderDTO>()
            {
                Value = await _ordersService.GetOrderById(Id)
            };
        }
    }
}
