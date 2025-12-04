using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.BLL.Dtos;

namespace ApiOrdenes.BLL.Servicios
{
    public interface IOrdenesServicio
    {
        Task<CustomResponse<List<OrdenDto>>> ObtenerOrdenesAsync();
        Task<CustomResponse<OrdenDto>> ObtenerOrdenPorIdAsync(int id);
        Task<CustomResponse<OrdenDto>> CrearOrdenAsync(OrdenDto ordenDto);
        Task<CustomResponse<bool>> CancelarOrdenAsync(int id);
    }
}