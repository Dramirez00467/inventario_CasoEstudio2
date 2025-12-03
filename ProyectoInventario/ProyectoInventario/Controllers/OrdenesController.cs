using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioBLL.Dtos;
using ProyectoInventarioBLL.Servicios;

namespace ProyectoInventario.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ILogger<OrdenesController> _logger;
        private readonly IOrdenesServicio _ordenesServicio;

        public OrdenesController(ILogger<OrdenesController> logger, IOrdenesServicio ordenesServicio)
        {
            _logger = logger;
            _ordenesServicio = ordenesServicio;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Listado de Órdenes";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenes()
        {
            var respuesta = await _ordenesServicio.ObtenerOrdenesAsync();
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearOrden(OrdenDto ordenDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new CustomResponse<OrdenDto>
                {
                    EsError = true,
                    Mensaje = "Error de validación"
                });
            }

            var respuesta = await _ordenesServicio.AgregarOrdenAsync(ordenDto);
            return Json(respuesta);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenPorId(int id)
        {
            var respuesta = await _ordenesServicio.ObtenerOrdenPorIdAsync(id);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarOrden(int id)
        {
            var respuesta = await _ordenesServicio.CancelarOrdenAsync(id);
            return Json(respuesta);
        }

        public IActionResult ConsultaPorId()
        {
            return View();
        }
    }
}