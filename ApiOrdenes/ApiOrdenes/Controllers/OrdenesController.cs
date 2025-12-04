using ApiOrdenes.BLL.Dtos;
using ApiOrdenes.BLL.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiOrdenes.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class OrdenesController : Controller
        {
            private readonly IOrdenesServicio _ordenesServicio;

            public OrdenesController(IOrdenesServicio ordenesServicio)
            {
                _ordenesServicio = ordenesServicio;
            }

            [HttpGet(Name = "ObtenerOrdenes")]
            public async Task<IActionResult> ObtenerOrdenes()
            {
                var respuesta = await _ordenesServicio.ObtenerOrdenesAsync();
                return Ok(respuesta);
            }

            // Ver detalle de una orden
            [HttpGet("{id}", Name = "ObtenerOrdenPorId")]
            public async Task<IActionResult> ObtenerOrdenPorId(int id)
            {
                var respuesta = await _ordenesServicio.ObtenerOrdenPorIdAsync(id);
                if (respuesta.Data is null)
                {
                    return NotFound("Orden no encontrada");
                }
                return Ok(respuesta);
            }

            // Crear orden
            [HttpPost(Name = "CrearOrden")]
            public async Task<IActionResult> CrearOrden(OrdenDto orden)
            {
                var respuesta = await _ordenesServicio.CrearOrdenAsync(orden);

                if (respuesta.EsError)
                {
                    return BadRequest(respuesta.Mensaje);
                }

                return Ok(respuesta);
            }

            // Cancelar orden
            [HttpPut("{id}/cancelar", Name = "CancelarOrden")]
            public async Task<IActionResult> CancelarOrden(int id)
            {
                var respuesta = await _ordenesServicio.CancelarOrdenAsync(id);

                if (respuesta.EsError)
                {
                    return BadRequest(respuesta.Mensaje);
                }

                return Ok(respuesta);
            }
        }
    }