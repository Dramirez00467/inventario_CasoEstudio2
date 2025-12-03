using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.DLL.Entidades;

namespace ApiInventario.DLL.Repositorio
{
    public interface IOrdenesRepositorio
    {
        Task<List<Orden>> ObtenerOrdenesAsync();
        Task<Orden> ObtenerOrdenPorIdAsync(int id);
        Task<bool> AgregarOrdenAsync(Orden orden);
        Task<bool> CancelarOrdenAsync(int id);
    }
}