using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOrdenes.DLL.Entidades
{
    public class Orden
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; }
    }
}