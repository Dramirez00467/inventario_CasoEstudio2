using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoInventarioBLL.Dtos;

namespace ProyectoInventarioBLL.Servicios
{
    public interface IProductosServicio
    {
        Task<CustomResponse<ProductoDto>> ObtenerProductoPorIdAsync(int id);
        Task<CustomResponse<List<ProductoDto>>> ObtenerProductosAsync();
        Task<CustomResponse<ProductoDto>> AgregarProductoAsync(ProductoDto productoDto);
        Task<CustomResponse<ProductoDto>> ActualizarProductoAsync(ProductoDto productoDto);
        Task<CustomResponse<ProductoDto>> EliminarProductoAsync(int id);
        Task<CustomResponse<bool>> ActualizarStockAsync(int idProducto, int cantidad);
        Task<CustomResponse<bool>> HayStockAsync(int idProducto, int cantidad);
    }
}