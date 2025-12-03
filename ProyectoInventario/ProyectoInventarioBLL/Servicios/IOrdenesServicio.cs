using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoInventarioBLL.Dtos;

namespace ProyectoInventarioBLL.Servicios
{
    public interface IOrdenesServicio
    {
        Task<CustomResponse<OrdenDto>> ObtenerOrdenPorIdAsync(int id);
        Task<CustomResponse<List<OrdenDto>>> ObtenerOrdenesAsync();
        Task<CustomResponse<OrdenDto>> AgregarOrdenAsync(OrdenDto ordenDto);
        Task<CustomResponse<OrdenDto>> CancelarOrdenAsync(int id);
    }
}