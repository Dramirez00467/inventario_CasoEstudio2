using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.DLL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiOrdenes.DLL.Repositorio
{
    public class OrdenesRepositorio : IOrdenesRepositorio
    {
        private readonly ApiContext _context;

        public OrdenesRepositorio(ApiContext context)
        {
            _context = context;
        }

        public async Task<List<Orden>> ObtenerOrdenesAsync()
        {
            return await _context.Ordenes.ToListAsync();
        }

        public async Task<Orden> ObtenerOrdenPorIdAsync(int id)
        {
            var orden = _context.Ordenes.FirstOrDefault(v => v.Id == id);
            return orden;
        }

        public async Task<bool> AgregarOrdenAsync(Orden orden)
        {
            await _context.Ordenes.AddAsync(orden);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CancelarOrdenAsync(int id)
        {
            var orden = _context.Ordenes.FirstOrDefault(v => v.Id == id);

            orden.Estado = "Cancelada";
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}