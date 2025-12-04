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
    public class ProductosServicio : IProductosServicio
    {
        // Inyección de dependencias
        private readonly IProductosRepositorio _productosRepositorio;
        private readonly IMapper _mapper;

        public ProductosServicio(IProductosRepositorio productosRepositorio, IMapper mapper)
        {
            _productosRepositorio = productosRepositorio;
            _mapper = mapper;
        }

   
        public async Task<CustomResponse<List<ProductoDto>>> ObtenerProductosAsync()
        {
            var respuesta = new CustomResponse<List<ProductoDto>>();

            var productos = await _productosRepositorio.ObtenerProductosAsync();
            var productosDto = _mapper.Map<List<ProductoDto>>(productos);
            respuesta.Data = productosDto;

            return respuesta;
        }

        public async Task<CustomResponse<ProductoDto>> ObtenerProductoPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<ProductoDto>();

            var producto = await _productosRepositorio.ObtenerProductoPorIdAsync(id);

            respuesta.Data = _mapper.Map<ProductoDto>(producto);

            return respuesta;
        }

        public async Task<CustomResponse<ProductoDto>> CrearProductoAsync(ProductoDto productoDto)
        {
            var respuesta = new CustomResponse<ProductoDto>();

            if (!await _productosRepositorio.AgregarProductoAsync(_mapper.Map<Producto>(productoDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo crear el producto";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<ProductoDto>> ActualizarProductoAsync(ProductoDto productoDto)
        {
            var respuesta = new CustomResponse<ProductoDto>();

            var producto = _mapper.Map<Producto>(productoDto);

            if (!await _productosRepositorio.ActualizarProductoAsync(producto))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el producto";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<bool>> EliminarProductoAsync(int id)
        {
            var respuesta = new CustomResponse<bool>();

            if (!await _productosRepositorio.EliminarProductoAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar el producto";
                respuesta.Data = false;
                return respuesta;
            }

            return respuesta;
        }

        // Actualizar el stock de un producto
        public async Task<CustomResponse<bool>> ActualizarStockAsync(int idProducto, int cantidad)
        {
            var respuesta = new CustomResponse<bool>();

            if (!await _productosRepositorio.ActualizarStockAsync(idProducto, cantidad))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el stock";
                respuesta.Data = false;
                return respuesta;
            }

            respuesta.Data = true;
            return respuesta;
        }

        // Verificar si hay stock suficiente para un producto
        public async Task<CustomResponse<bool>> HayStockAsync(int idProducto, int cantidad)
        {
            var respuesta = new CustomResponse<bool>();

            var hayStock = await _productosRepositorio.HayStockAsync(idProducto, cantidad);

            respuesta.Data = hayStock;

            if (!hayStock)
            {
                respuesta.Mensaje = "No hay stock suficiente o el producto no está disponible";
            }

            return respuesta;
        }
    }
}