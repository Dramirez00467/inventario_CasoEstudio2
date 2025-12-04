using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.BLL.Dtos;

namespace ApiInventario.BLL.Servicios
{
    public interface IProductosServicio
    {
        Task<CustomResponse<List<ProductoDto>>> ObtenerProductosAsync();
        Task<CustomResponse<ProductoDto>> ObtenerProductoPorIdAsync(int id);
        Task<CustomResponse<ProductoDto>> CrearProductoAsync(ProductoDto productoDto);
        Task<CustomResponse<ProductoDto>> ActualizarProductoAsync(ProductoDto productoDto);
        Task<CustomResponse<bool>> EliminarProductoAsync(int id);

        // Actualizar el stock de un producto
        Task<CustomResponse<bool>> ActualizarStockAsync(int idProducto, int cantidad);

        // Verificar si hay stock suficiente para un producto
        Task<CustomResponse<bool>> HayStockAsync(int idProducto, int cantidad);
    }
}