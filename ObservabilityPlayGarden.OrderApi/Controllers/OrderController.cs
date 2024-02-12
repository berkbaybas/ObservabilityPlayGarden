using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObservabilityPlayGarden.OrderApi.DTOs;
using ObservabilityPlayGarden.OrderApi.Services;

namespace ObservabilityPlayGarden.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult Create(OrderCreateRequestDTO dto)
        {
            #region Exception örneği için
            // Exception örneği için
            //var a = 10;
            //var b = 0;
            //var c = a / b;
            #endregion

            orderService.CreateAsync(dto);

            return Ok();
        }
    }
}
