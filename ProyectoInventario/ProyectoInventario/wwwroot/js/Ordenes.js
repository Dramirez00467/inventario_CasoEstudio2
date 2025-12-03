// Aislar funciones 
(() => {
    const Ordenes = {
        tabla: null,
        productosDiccionario: {},

        init() {
            this.inicializarTabla();
            this.cargarProductos();
            this.registrarEventos();
        },

        inicializarTabla() {
            this.tabla = $('#tablaOrdenes').DataTable({
                ajax: {
                    url: '/Ordenes/ObtenerOrdenes',
                    type: 'GET',
                    dataSrc: 'data',
                    error: function (exceptionResponse) {
                        Swal.fire({
                            title: 'Error',
                            text: exceptionResponse.responseJSON?.mensaje ?? 'Error al cargar las órdenes',
                            icon: 'error',
                        });
                    }
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    {
                        data: 'productoId',
                        title: 'Producto',
                        render: (data) => {
                            return Ordenes.obtenerNombreProducto(data);
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    {
                        data: 'fecha',
                        title: 'Fecha',
                        render: function (data) {
                            return Ordenes.formatearFecha(data);
                        }
                    },
                    { data: 'estado', title: 'Estado' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                <button class="btn btn-sm btn-info detalles" data-id="${row.id}">Detalles</button>
                                <button class="btn btn-sm btn-danger eliminar" data-id="${row.id}">Cancelar orden</button>
                            `;
                        }
                    }
                ],
                responsive: true,
                processing: true,
                pageLength: 10
            });
        },

        cargarProductos() {
            $.ajax({
                url: '/Productos/ObtenerProductos',
                type: 'GET',
                success: function (response) {
                    if (!response.esError && response.data) {
                        let opciones = '<option value="">Seleccione un producto</option>';

                        response.data.forEach(p => {
                            opciones += `<option value="${p.id}">${p.nombre}</option>`;
                            Ordenes.productosDiccionario[p.id] = p.nombre;
                        });

                        $('#ProductoId, #ProductoIdEditar').html(opciones);
                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: response.mensaje ?? 'No se pudo cargar la lista de productos',
                            icon: 'error'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'No se pudo obtener la lista de productos',
                        icon: 'error'
                    });
                }
            });
        },

        registrarEventos() {

            $('#tablaOrdenes').on('click', '.detalles', function () {
                const id = $(this).data('id');
                Ordenes.MostrarDetallesOrden(id);
            });

            $('#tablaOrdenes').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Ordenes.CancelarOrden(id);
            });

            $('#btnGuardarOrden').on('click', function () {
                Ordenes.GuardarOrden();
            });

            $('#btnEditarOrden').on('click', function () {
                Ordenes.EditarOrden();
            });

            $('#btnBuscarOrden').on('click', function () {
                Ordenes.BuscarPorId();
            });
        },

        GuardarOrden: function () {
            let form = $('#formCrearOrden');

            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (response.esError) {
                        Swal.fire({
                            title: 'Error',
                            text: response.mensaje ?? 'No se pudo crear la orden (puede que el producto no tenga stock suficiente o esté inactivo).',
                            icon: 'error',
                        });
                        return;
                    }

                    $('#modalCrearOrden').modal('hide');
                    Ordenes.tabla.ajax.reload();
                    form[0].reset();

                    Swal.fire({
                        title: 'Éxito',
                        text: response.mensaje ?? 'Orden creada correctamente',
                        icon: 'success',
                    });
                },
                error: function (exceptionResponse) {
                    Swal.fire({
                        title: 'Error',
                        text: exceptionResponse.responseJSON?.mensaje ?? 'Error al crear la orden',
                        icon: 'error',
                    });
                }
            });
        },

        CargaDatosOrden: function (id) {
            $.get(`/Ordenes/ObtenerOrdenPorId/${id}`, function (response) {
                if (!response.esError && response.data) {
                    const orden = response.data;

                    $('#OrdenId').val(orden.id);
                    $('#ProductoIdEditar').val(orden.productoId);
                    $('#CantidadEditar').val(orden.cantidad);
                    $('#FechaEditar').val(orden.fecha);
                    $('#EstadoEditar').val(orden.estado);

                    $('#modalEditarOrden').modal('show');
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.mensaje ?? 'No se pudo cargar la orden',
                        icon: 'error',
                    });
                }
            }).fail(function () {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudo obtener la información de la orden',
                    icon: 'error',
                });
            });
        },

        EditarOrden: function () {
            let form = $('#formEditarOrden');

            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalEditarOrden').modal('hide');
                        Ordenes.tabla.ajax.reload();
                        Swal.fire({
                            title: 'Éxito',
                            text: response.mensaje ?? 'Orden actualizada correctamente',
                            icon: 'success',
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            text: response.mensaje,
                            icon: 'error',
                        });
                    }
                },
                error: function (exceptionResponse) {
                    Swal.fire({
                        title: 'Error',
                        text: exceptionResponse.responseJSON?.mensaje ?? 'Error al actualizar la orden',
                        icon: 'error',
                    });
                }
            });
        },

        CancelarOrden: function (id) {
            Swal.fire({
                title: "¿Estás seguro?",
                text: "La orden será marcada como cancelada.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, cancelar orden"
            }).then((result) => {

                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Ordenes/CancelarOrden',
                        type: 'POST',
                        data: { id: id },
                        success: function (response) {
                            if (!response.esError) {
                                Ordenes.tabla.ajax.reload();
                                Swal.fire({
                                    title: 'Éxito',
                                    text: response.mensaje ?? 'Orden cancelada correctamente',
                                    icon: 'success',
                                });
                            }
                            else {
                                Swal.fire({
                                    title: 'Error',
                                    text: response.mensaje,
                                    icon: 'error',
                                });
                            }
                        },
                        error: function (exceptionResponse) {
                            Swal.fire({
                                title: 'Error',
                                text: exceptionResponse.responseJSON?.mensaje ?? 'Error al cancelar la orden',
                                icon: 'error',
                            });
                        }
                    });
                }
            });
        },

        BuscarPorId: function () {

            const id = $('#buscarIdOrden').val();

            if (!id) {
                Swal.fire({
                    title: 'Advertencia',
                    text: 'Debe ingresar un ID',
                    icon: 'warning'
                });
                return;
            }

            $.ajax({
                url: `/Ordenes/ObtenerOrdenPorId/${id}`,
                type: 'GET',
                success: function (response) {
                    if (response.esError || !response.data) {
                        $('#resultadoOrden').hide();
                        Swal.fire({
                            title: 'No encontrado',
                            text: 'No existe una orden con ese ID',
                            icon: 'error'
                        });
                        return;
                    }

                    const o = response.data;

                    $('#resIdOrden').text(o.id);
                    $('#resProductoIdOrden').text(Ordenes.obtenerNombreProducto(o.productoId));
                    $('#resCantidadOrden').text(o.cantidad);
                    $('#resFechaOrden').text(Ordenes.formatearFecha(o.fecha));
                    $('#resEstadoOrden').text(o.estado);

                    $('#resultadoOrden').show();
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'No se pudo obtener la información',
                        icon: 'error'
                    });
                }
            });

        },

        MostrarDetallesOrden: function (id) {
            $.get(`/Ordenes/ObtenerOrdenPorId/${id}`, function (response) {
                if (!response.esError && response.data) {
                    const o = response.data;

                    $('#detOrdId').text(o.id);
                    $('#detOrdProducto').text(Ordenes.obtenerNombreProducto(o.productoId));
                    $('#detOrdCantidad').text(o.cantidad);
                    $('#detOrdFecha').text(Ordenes.formatearFecha(o.fecha));
                    $('#detOrdEstado').text(o.estado);

                    $('#modalDetalleOrden').modal('show');
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.mensaje ?? 'No se pudo cargar el detalle de la orden',
                        icon: 'error',
                    });
                }
            }).fail(function () {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudo obtener la información de la orden',
                    icon: 'error',
                });
            });
        },

        obtenerNombreProducto: function (idProducto) {
            if (!idProducto) return '';
            return this.productosDiccionario[idProducto] ?? `ID ${idProducto}`;
        },

        formatearFecha: function (fechaStr) {
            if (!fechaStr) return '';
            const fecha = new Date(fechaStr);
            if (isNaN(fecha.getTime())) return fechaStr;

            const dia = fecha.getDate().toString().padStart(2, '0');
            const mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
            const anio = fecha.getFullYear();

            return `${dia}/${mes}/${anio}`;
        }

    }

    $(document).ready(() => Ordenes.init());
})();
