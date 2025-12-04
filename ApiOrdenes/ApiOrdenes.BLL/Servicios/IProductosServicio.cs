using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.BLL.Dtos;

namespace ApiOrdenes.BLL.Servicios
{
    public interface IProductosServicio
    {
        Task<CustomResponse<bool>> HayStockAsync(int idProducto, int cantidad);
        Task<CustomResponse<bool>> ActualizarStockAsync(int idProducto, int cantidad);
    }
}