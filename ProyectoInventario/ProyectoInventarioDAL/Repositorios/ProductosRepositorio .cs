using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ProyectoInventarioDAL.Entidades;
using ProyectoInventarioDAL.RespuestasAPIS;

namespace ProyectoInventarioDAL.Repositorios
{
    public class ProductosRepositorio : IProductosRepositorio
    {

        private readonly HttpClient _httpClient;

        public ProductosRepositorio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            var informacion = new StringContent(
                   System.Text.Json.JsonSerializer.Serialize(producto),
                   Encoding.UTF8,
                   "application/json"
               );

            var response = await _httpClient.PutAsync("https://localhost:7288/Productos", informacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AgregarProductoAsync(Producto producto)
        {
            var informacion = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(producto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://localhost:7288/Productos", informacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7288/Productos/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<Producto> ObtenerProductoPorIdAsync(int id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<RespuestaApiInventario<Producto>>($"https://localhost:7288/Productos/{id}");

            return response?.Data;
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            var response = await _httpClient
                .GetFromJsonAsync<RespuestaApiInventario<List<Producto>>>("https://localhost:7288/Productos");

            return response?.Data ?? new List<Producto>();
        }

        public async Task<bool> ActualizarStockAsync(int idProducto, int cantidad)
        {
            var informacion = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(new { cantidad }),
                Encoding.UTF8,
                "application/json"
            );

            var url = $"https://localhost:7288/Productos/{idProducto}/stock?cantidad={cantidad}";

            var response = await _httpClient.PutAsync(url, informacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> HayStockAsync(int idProducto, int cantidad)
        {
            var url = $"https://localhost:7288/Productos/{idProducto}/hay-stock?cantidad={cantidad}";

            var response = await _httpClient
                .GetFromJsonAsync<RespuestaApiInventario<bool>>(url);

            return response?.Data ?? false;
        }
    }
}