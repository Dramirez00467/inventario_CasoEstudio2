using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProyectoInventarioBLL.Dtos;
using ProyectoInventarioDAL.Entidades;
using ProyectoInventarioDAL.Repositorios;

namespace ProyectoInventarioBLL.Servicios
{
    public class OrdenesServicio : IOrdenesServicio
    {
        //Inyección de dependencias
        private readonly IOrdenesRepositorio _ordenesRepositorio;
        private readonly IMapper _mapper;

        public OrdenesServicio(IOrdenesRepositorio ordenesRepositorio, IMapper mapper)
        {
            _ordenesRepositorio = ordenesRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<OrdenDto>> AgregarOrdenAsync(OrdenDto ordenDto)
        {
            var respuesta = new CustomResponse<OrdenDto>();

            if (!await _ordenesRepositorio.AgregarOrdenAsync(_mapper.Map<Orden>(ordenDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No fue posible agregar el registro";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<OrdenDto>> CancelarOrdenAsync(int id)
        {
            var respuesta = new CustomResponse<OrdenDto>();

            if (!await _ordenesRepositorio.CancelarOrdenAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No fue posible cancelar el registro";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<OrdenDto>> ObtenerOrdenPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<OrdenDto>();

            var orden = await _ordenesRepositorio.ObtenerOrdenPorIdAsync(id);
            respuesta.Data = _mapper.Map<OrdenDto>(orden);

            return respuesta;
        }

        public async Task<CustomResponse<List<OrdenDto>>> ObtenerOrdenesAsync()
        {
            var respuesta = new CustomResponse<List<OrdenDto>>();

            var ordenes = await _ordenesRepositorio.ObtenerOrdenesAsync();
            var ordenesDto = _mapper.Map<List<OrdenDto>>(ordenes);
            respuesta.Data = ordenesDto;

            return respuesta;
        }
    }
}