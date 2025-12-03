using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiInventario.BLL.Dtos;
using ApiInventario.DLL.Entidades;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiInventario.BLL.Mapeos
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {

            //Productos
            CreateMap<Producto, ProductoDto>();
            CreateMap<ProductoDto, Producto>();

            // Ordenes
            CreateMap<Orden, OrdenDto>();
            CreateMap<OrdenDto, Orden>();
        }
    }
}