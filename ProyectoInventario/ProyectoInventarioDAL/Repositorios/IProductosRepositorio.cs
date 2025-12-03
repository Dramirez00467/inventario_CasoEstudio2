using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoInventarioDAL.Entidades;

namespace ProyectoInventarioDAL.Repositorios
{
    public interface IProductosRepositorio
    {
        Task<List<Producto>> ObtenerProductosAsync();
        Task<Producto> ObtenerProductoPorIdAsync(int id);
        Task<bool> AgregarProductoAsync(Producto producto);
        Task<bool> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int id);
        Task<bool> ActualizarStockAsync(int idProducto, int cantidad);
        Task<bool> HayStockAsync(int idProducto, int cantidad);
    }
}