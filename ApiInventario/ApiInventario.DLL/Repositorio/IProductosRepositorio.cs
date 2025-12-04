using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.DLL.Entidades;

namespace ApiInventario.DLL.Repositorio
{
    public interface IProductosRepositorio
    {
        Task<List<Producto>> ObtenerProductosAsync();
        Task<Producto?> ObtenerProductoPorIdAsync(int id);
        Task<bool> AgregarProductoAsync(Producto producto);
        Task<bool> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int id);

        // Actualizar el stock de un producto
        Task<bool> ActualizarStockAsync(int idProducto, int cantidad);

        // Verificar si hay stock suficiente para un producto
        Task<bool> HayStockAsync(int idProducto, int cantidad);
    }
}