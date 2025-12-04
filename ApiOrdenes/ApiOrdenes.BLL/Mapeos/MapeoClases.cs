using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiOrdenes.BLL.Dtos;
using ApiOrdenes.DLL.Entidades;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiOrdenes.BLL.Mapeos
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            // Ordenes
            CreateMap<Orden, OrdenDto>();
            CreateMap<OrdenDto, Orden>();
        }
    }
}