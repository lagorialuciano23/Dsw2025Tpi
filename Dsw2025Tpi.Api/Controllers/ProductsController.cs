using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private IProductsManagementService _productsManagmentService;

        public ProductsController(IProductsManagementService _ProductService)
        {
            _productsManagmentService = _ProductService;
        }
        //Agregar un producto
        [HttpPost()]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel.ProductRequest request)
        {
            try
            {
                var product = await _productsManagmentService.AddProduct(request);
                return StatusCode(201, product);
            }
            catch (ArgumentException)
            {
                var validationErrors = new Dictionary<string, string[]>
    {
        { "Sku", new[] { "El campo SKU es obligatorio." } },
        { "Name", new[] { "El nombre del producto es obligatorio." } },
        { "CurrentUnitPrice", new[] { "El precio debe ser mayor a 0." } },
        { "StockQuantity", new[] { "El stock no puede ser negativo." } }
    };

                return BadRequest(new
                {
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors = validationErrors
                });
            }
            //catch (ArgumentException ae)
            //{
            //    return BadRequest(ae.Message);
            //}
            catch (ApplicationException de)
            {
                return Conflict(de.Message);
            }
            //catch (Exception)
            //{
            //    return Problem("Se produjo un error al guardar el producto");
            //}
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al guardar el producto",
                    detail = ex.Message,
                    trace = ex.StackTrace
                });
            }
        }
        //Obtener todos los productos
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productsManagmentService.GetProducts();

            if (!products.Any())
                return NoContent(); // Devuelve 204

            return Ok(products);  //Devuelve 200
        }
        //Obtener producto por Id
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productsManagmentService.GetProductById(id);
            if (product is null)
            {
                return NotFound(); //Devuelve un 404 not found
            }
            return Ok(product); // Devuelve un 200 ok
        }
        //Modificar un producto por Id
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductModel.ProductRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Sku) ||
                string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Name)
                )
            {
                return BadRequest();
            }
            try
            {
                var response = await _productsManagmentService.UpdateAsync(request, id);
                return Ok(response);
            }
            catch (ArgumentException)
            {
                var validationErrors = new Dictionary<string, string[]>
    {
        { "Sku", new[] { "El campo SKU es obligatorio." } },
        { "Name", new[] { "El nombre del producto es obligatorio." } },
        { "Price", new[] { "El precio debe ser mayor a 0." } },
        { "Stock", new[] { "El stock no puede ser negativo." } }
    };

                return BadRequest(new
                {
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors = validationErrors
                });
            }
        }
        //Inhabilitamos un articulo Logicamente con el isActive
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> Disable(Guid id)
        {
            var wasDisabled = await _productsManagmentService.DisableProductAsync(id);
            if (!wasDisabled)
                return NotFound();

            return NoContent(); // Me devuelve un 204
        }
    }
}
