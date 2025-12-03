using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.BLL.Dtos;
using ApiInventario.DLL.Entidades;
using ApiInventario.DLL.Repositorio;
using AutoMapper;

namespace ApiInventario.BLL.Servicios
{
    public class OrdenesServicio : IOrdenesServicio
    {
        private readonly IOrdenesRepositorio _ordenesRepositorio;
        private readonly IProductosServicio _productosServicio;
        private readonly IMapper _mapper;

        public OrdenesServicio(
            IOrdenesRepositorio ordenesRepositorio,
            IProductosServicio productosServicio,
            IMapper mapper)
        {
            _ordenesRepositorio = ordenesRepositorio;
            _productosServicio = productosServicio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<List<OrdenDto>>> ObtenerOrdenesAsync()
        {
            var respuesta = new CustomResponse<List<OrdenDto>>();

            var ordenes = await _ordenesRepositorio.ObtenerOrdenesAsync();
            var ordenesDto = _mapper.Map<List<OrdenDto>>(ordenes);
            respuesta.Data = ordenesDto;

            return respuesta;
        }

        public async Task<CustomResponse<OrdenDto>> ObtenerOrdenPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<OrdenDto>();

            var orden = await _ordenesRepositorio.ObtenerOrdenPorIdAsync(id);

            respuesta.Data = _mapper.Map<OrdenDto>(orden);

            return respuesta;
        }

        public async Task<CustomResponse<OrdenDto>> CrearOrdenAsync(OrdenDto ordenDto)
        {
            var respuesta = new CustomResponse<OrdenDto>();

            // Valida el stock con el servicio de productos
            var validaStock = await _productosServicio.HayStockAsync(ordenDto.ProductoId, ordenDto.Cantidad);

            if (!validaStock.Data)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No hay stock suficiente o el producto no está disponible";
                return respuesta;
            }

            // Baja el stock
            var actualizaStock = await _productosServicio.ActualizarStockAsync(ordenDto.ProductoId, -ordenDto.Cantidad);

            if (actualizaStock.EsError)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el stock del producto";
                return respuesta;
            }

            // Crea y guarda la orden
            var entidad = _mapper.Map<Orden>(ordenDto);
            entidad.Fecha = DateTime.Now;
            entidad.Estado = "Activa";

            if (!await _ordenesRepositorio.AgregarOrdenAsync(entidad))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo crear la orden";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<OrdenDto>(entidad);
            return respuesta;
        }

        public async Task<CustomResponse<bool>> CancelarOrdenAsync(int id)
        {
            var respuesta = new CustomResponse<bool>();

            // Recupera la orden para devolver stock 
            var orden = await _ordenesRepositorio.ObtenerOrdenPorIdAsync(id);

            if (orden == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La orden no existe";
                respuesta.Data = false;
                return respuesta;
            }

            // Si la orden está cancelada, no hace nada
            if (orden.Estado == "Cancelada")
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La orden ya está cancelada";
                respuesta.Data = false;
                return respuesta;
            }

            // Devuelve el stock
            var actualizaStock = await _productosServicio.ActualizarStockAsync(orden.ProductoId, orden.Cantidad);

            if (actualizaStock.EsError)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el stock al cancelar la orden";
                respuesta.Data = false;
                return respuesta;
            }

            // Marca la orden como cancelada
            if (!await _ordenesRepositorio.CancelarOrdenAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo cancelar la orden";
                respuesta.Data = false;
                return respuesta;
            }

            respuesta.Data = true;
            return respuesta;
        }
    }
}