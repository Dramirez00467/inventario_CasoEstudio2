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
    public class OrdenesRepositorio : IOrdenesRepositorio
    {

        private readonly HttpClient _httpClient;

        public OrdenesRepositorio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AgregarOrdenAsync(Orden orden)
        {
            var informacion = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(orden),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://localhost:7286/Ordenes", informacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelarOrdenAsync(int id)
        {
            var informacion = new StringContent(
                   System.Text.Json.JsonSerializer.Serialize(new { }),
                   Encoding.UTF8,
                   "application/json"
               );

            var url = $"https://localhost:7286/Ordenes/{id}/cancelar";

            var response = await _httpClient.PutAsync(url, informacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<Orden> ObtenerOrdenPorIdAsync(int id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<RespuestaApiInventario<Orden>>($"https://localhost:7286/Ordenes/{id}");

            return response?.Data;
        }

        public async Task<List<Orden>> ObtenerOrdenesAsync()
        {
            var response = await _httpClient
                .GetFromJsonAsync<RespuestaApiInventario<List<Orden>>>("https://localhost:7286/Ordenes");

            return response?.Data ?? new List<Orden>();
        }
    }
}