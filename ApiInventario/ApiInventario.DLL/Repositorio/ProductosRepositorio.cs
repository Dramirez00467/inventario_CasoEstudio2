using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.DLL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.DLL.Repositorio
{
    public class ProductosRepositorio : IProductosRepositorio
    {
        private readonly ApiContext _context;

        public ProductosRepositorio(ApiContext context)
        {
            _context = context;
        }

        /*****COPY PASTE*****/
        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            var productoActualizar = _context.Productos.FirstOrDefault(v => v.Id == producto.Id); // recupera el objeto

            productoActualizar.Nombre = producto.Nombre;       // actualiza la información
            productoActualizar.Descripcion = producto.Descripcion;
            productoActualizar.Precio = producto.Precio;
            productoActualizar.Stock = producto.Stock;
            productoActualizar.EstaDisponible = producto.EstaDisponible;

            var result = await _context.SaveChangesAsync();     // aplica los cambios

            return result > 0; // check si funcionó
        }

        public async Task<bool> AgregarProductoAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            var producto = _context.Productos.FirstOrDefault(v => v.Id == id);

            _context.Productos.Remove(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Producto> ObtenerProductoPorIdAsync(int id)
        {
            //SP //API // ETC
            var producto = _context.Productos.FirstOrDefault(v => v.Id == id);
            return producto;
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        // Actualiza el stock sumando o restando la cantidad indicada
        public async Task<bool> ActualizarStockAsync(int idProducto, int cantidad)
        {
            var producto = _context.Productos.FirstOrDefault(v => v.Id == idProducto); // recupera el objeto

            producto.Stock = producto.Stock + cantidad;  // suma o resta el stock

            var result = await _context.SaveChangesAsync(); // aplica cambios

            return result > 0;
        }


        // Verifica si hay stock suficiente para la cantidad indicada
        public async Task<bool> HayStockAsync(int idProducto, int cantidad)
        {
            var producto = _context.Productos.FirstOrDefault(v => v.Id == idProducto); // recupera objeto

            if (!producto.EstaDisponible)
            {
                return false;
            }

            return producto.Stock >= cantidad; // devuelve true/false
        }
    }
}