using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioBLL.Dtos;
using ProyectoInventarioBLL.Servicios;

namespace ProyectoInventario.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly IProductosServicio _productosServicio;

        public ProductosController(ILogger<ProductosController> logger, IProductosServicio productosServicio)
        {
            _logger = logger;
            _productosServicio = productosServicio;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Listado de Productos";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            var respuesta = await _productosServicio.ObtenerProductosAsync();
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProducto(ProductoDto productoDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new CustomResponse<ProductoDto>
                {
                    EsError = true,
                    Mensaje = "Error de validación"
                });
            }

            var respuesta = await _productosServicio.AgregarProductoAsync(productoDto);
            return Json(respuesta);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductoPorId(int id)
        {
            var respuesta = await _productosServicio.ObtenerProductoPorIdAsync(id);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProducto(ProductoDto productoDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new CustomResponse<ProductoDto>
                {
                    EsError = true,
                    Mensaje = "Error de validación"
                });
            }

            var respuesta = await _productosServicio.ActualizarProductoAsync(productoDto);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var respuesta = await _productosServicio.EliminarProductoAsync(id);
            return Json(respuesta);
        }

        public IActionResult ConsultaPorId()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarStock(int idProducto, int cantidad)
        {
            var respuesta = await _productosServicio.ActualizarStockAsync(idProducto, cantidad);
            return Json(respuesta);
        }

        [HttpGet]
        public async Task<IActionResult> HayStock(int idProducto, int cantidad)
        {
            var respuesta = await _productosServicio.HayStockAsync(idProducto, cantidad);
            return Json(respuesta);
        }
    }
}