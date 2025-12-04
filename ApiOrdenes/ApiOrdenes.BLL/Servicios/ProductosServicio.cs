using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.BLL.Dtos;

namespace ApiOrdenes.BLL.Servicios
{
    public class ProductosServicio : IProductosServicio
    {
        private readonly HttpClient _httpClient;

        public ProductosServicio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponse<bool>> HayStockAsync(int idProducto, int cantidad)
        {
            var response = await _httpClient.GetAsync(
                $"Productos/{idProducto}/hay-stock?cantidad={cantidad}");

            if (!response.IsSuccessStatusCode)
            {
                return new CustomResponse<bool>
                {
                    EsError = true,
                    Mensaje = "Error llamando API de Inventario",
                    Data = false
                };
            }

            var resultado = await response.Content
                .ReadFromJsonAsync<CustomResponse<bool>>();

            // Si vino nulo, devolvemos error amigable
            return resultado ?? new CustomResponse<bool>
            {
                EsError = true,
                Mensaje = "Respuesta inválida del API de Inventario",
                Data = false
            };
        }

        public async Task<CustomResponse<bool>> ActualizarStockAsync(int idProducto, int cantidad)
        {
            var response = await _httpClient.PutAsync(
                $"Productos/{idProducto}/stock?cantidad={cantidad}",
                null); // cuerpo vacío

            if (!response.IsSuccessStatusCode)
            {
                return new CustomResponse<bool>
                {
                    EsError = true,
                    Mensaje = "Error actualizando stock en API de Inventario",
                    Data = false
                };
            }

            var resultado = await response.Content
                .ReadFromJsonAsync<CustomResponse<bool>>();

            return resultado ?? new CustomResponse<bool>
            {
                EsError = true,
                Mensaje = "Respuesta inválida del API de Inventario",
                Data = false
            };
        }
    }
}