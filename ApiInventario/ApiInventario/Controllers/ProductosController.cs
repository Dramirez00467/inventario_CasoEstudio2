using ApiInventario.BLL.Dtos;
using ApiInventario.BLL.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiInventario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : Controller
    {
        private readonly IProductosServicio _productosServicio;

        public ProductosController(IProductosServicio productosServicio)
        {
            _productosServicio = productosServicio;
        }

        // Listar productos
        [HttpGet(Name = "ObtenerProductos")]
        public async Task<IActionResult> ObtenerProductos()
        {
            var respuesta = await _productosServicio.ObtenerProductosAsync();
            return Ok(respuesta);
        }

        // Ver detalle de un producto
        [HttpGet("{id}", Name = "ObtenerProductoPorId")]
        public async Task<IActionResult> ObtenerProductoPorId(int id)
        {
            var respuesta = await _productosServicio.ObtenerProductoPorIdAsync(id);

            if (respuesta.Data == null)
            {
                return NotFound("Producto no encontrado");
            }

            return Ok(respuesta);
        }

        // Crear producto
        [HttpPost(Name = "CrearProducto")]
        public async Task<IActionResult> CrearProducto(ProductoDto producto)
        {
            var respuesta = await _productosServicio.CrearProductoAsync(producto);

            if (respuesta.EsError)
            {
                return BadRequest(respuesta.Mensaje);
            }

            return Ok(respuesta);
        }

        // Modificar producto (no stock)
        [HttpPut(Name = "ActualizarProducto")]
        public async Task<IActionResult> ActualizarProducto(ProductoDto producto)
        {
            var respuesta = await _productosServicio.ActualizarProductoAsync(producto);

            return Ok(respuesta);
        }

        // Eliminar producto
        [HttpDelete("{id}", Name = "EliminarProducto")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var respuesta = await _productosServicio.EliminarProductoAsync(id);

            return Ok(respuesta);
        }

        // Actualizar stock de un producto
        [HttpPut("{id}/stock", Name = "ActualizarStockProducto")]
        public async Task<IActionResult> ActualizarStockProducto(int id, int cantidad)
        {
            var respuesta = await _productosServicio.ActualizarStockAsync(id, cantidad);

            if (respuesta.EsError)
            {
                return BadRequest(respuesta.Mensaje);
            }

            return Ok(respuesta);
        }

        // Verificar si hay stock
        [HttpGet("{id}/hay-stock", Name = "HayStockProducto")]
        public async Task<IActionResult> HayStockProducto(int id, int cantidad)
        {
            var respuesta = await _productosServicio.HayStockAsync(id, cantidad);

            return Ok(respuesta);
        }
    }
}