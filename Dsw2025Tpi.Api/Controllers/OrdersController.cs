using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private IOrdersManagementService _orderManagmentService;
        public OrdersController(IOrdersManagementService orderManagement)
        {
            _orderManagmentService = orderManagement;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderModel.OrderRequest request)
        {
            try
            {
                var order = await _orderManagmentService.AddOrder(request);
                return Created("api/orders", order);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (EntityNotFoundException enf)
            {
                return NotFound(enf.Message);
            }
            catch (DuplicatedEntityException de)
            {
                return Conflict(de.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
