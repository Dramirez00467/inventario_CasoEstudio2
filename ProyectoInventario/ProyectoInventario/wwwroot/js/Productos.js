// Aislar funciones 
(() => {
    const Productos = {
        tabla: null,
        productoIdStockActual: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {
            this.tabla = $('#tablaProductos').DataTable({
                ajax: {
                    url: '/Productos/ObtenerProductos',
                    type: 'GET',
                    dataSrc: 'data',
                    error: function (exceptionResponse) {
                        Swal.fire({
                            title: 'Error',
                            text: exceptionResponse.responseJSON?.mensaje ?? 'Error al cargar los productos',
                            icon: 'error',
                        });
                    }
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'precio', title: 'Precio' },
                    { data: 'stock', title: 'Stock' },
                    {
                        data: 'estaDisponible',
                        title: 'Estado',
                        render: function (data) {
                            return data ? 'Activo' : 'Inactivo';
                        }
                    },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                <button class="btn btn-sm btn-info detalles" data-id="${row.id}">Detalles</button>
                                <button class="btn btn-sm btn-primary editar" data-id="${row.id}">Editar</button>
                                <button class="btn btn-sm btn-danger eliminar" data-id="${row.id}">Eliminar</button>
                                <button class="btn btn-sm btn-warning stock" data-id="${row.id}">Stock</button>
                            `;
                        }
                    }
                ],
                responsive: true,
                processing: true,
                pageLength: 10
            });
        },

        registrarEventos() {

            $('#tablaProductos').on('click', '.detalles', function () {
                const id = $(this).data('id');
                Productos.MostrarDetallesProducto(id);
            });

            $('#tablaProductos').on('click', '.editar', function () {
                const id = $(this).data('id');
                Productos.CargaDatosProducto(id);
            });

            $('#tablaProductos').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Productos.EliminarProducto(id);
            });

            $('#tablaProductos').on('click', '.stock', function () {
                const id = $(this).data('id');
                Productos.productoIdStockActual = id;
                $('#CantidadStock').val('');
                $('#modalStockProducto').modal('show');
            });

            $('#btnGuardarProducto').on('click', function () {
                Productos.GuardarProducto();
            });

            $('#btnEditarProducto').on('click', function () {
                Productos.EditarProducto();
            });

            $('#btnBuscarProducto').on('click', function () {
                Productos.BuscarPorId();
            });

            $('#btnSumarStock').on('click', function () {
                const cantidad = parseInt($('#CantidadStock').val());
                Productos.ActualizarStockProducto(Productos.productoIdStockActual, cantidad);
            });

            $('#btnRestarStock').on('click', function () {
                const cantidad = parseInt($('#CantidadStock').val());
                Productos.ActualizarStockProducto(Productos.productoIdStockActual, -cantidad);
            });
        },

        GuardarProducto: function () {
            let form = $('#formCrearProducto');

            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalCrearProducto').modal('hide');
                        Productos.tabla.ajax.reload();
                        form[0].reset();

                        Swal.fire({
                            title: 'Éxito',
                            text: response.mensaje ?? 'Producto creado correctamente',
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
                        text: exceptionResponse.responseJSON?.mensaje ?? 'Error al crear el producto',
                        icon: 'error',
                    });
                }
            });
        },

        CargaDatosProducto: function (id) {
            $.get(`/Productos/ObtenerProductoPorId/${id}`, function (response) {
                if (!response.esError && response.data) {
                    const producto = response.data;

                    $('#ProductoId').val(producto.id);
                    $('#modalEditarProducto #Nombre').val(producto.nombre);
                    $('#modalEditarProducto #Descripcion').val(producto.descripcion);
                    $('#modalEditarProducto #Precio').val(producto.precio);
                    $('#modalEditarProducto #Stock').val(producto.stock);
                    $('#modalEditarProducto #EstaDisponible').val(producto.estaDisponible.toString());

                    $('#modalEditarProducto').modal('show');
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.mensaje ?? 'No se pudo cargar el producto',
                        icon: 'error',
                    });
                }
            }).fail(function () {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudo obtener la información del producto',
                    icon: 'error',
                });
            });
        },

        EditarProducto: function () {
            let form = $('#formEditarProducto');

            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalEditarProducto').modal('hide');
                        Productos.tabla.ajax.reload();
                        Swal.fire({
                            title: 'Éxito',
                            text: response.mensaje ?? 'Producto actualizado correctamente',
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
                        text: exceptionResponse.responseJSON?.mensaje ?? 'Error al actualizar el producto',
                        icon: 'error',
                    });
                }
            });
        },

        EliminarProducto: function (id) {
            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta acción",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, borrar"
            }).then((result) => {

                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Productos/EliminarProducto',
                        type: 'POST',
                        data: { id: id },
                        success: function (response) {
                            if (!response.esError) {
                                Productos.tabla.ajax.reload();
                                Swal.fire({
                                    title: 'Éxito',
                                    text: response.mensaje ?? 'Producto eliminado correctamente',
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
                                text: exceptionResponse.responseJSON?.mensaje ?? 'Error al eliminar el producto',
                                icon: 'error',
                            });
                        }
                    });
                }
            });
        },

        ActualizarStockProducto: function (id, cantidad) {

            if (id == null) {
                Swal.fire({
                    title: 'Advertencia',
                    text: 'No se ha seleccionado un producto',
                    icon: 'warning'
                });
                return;
            }

            if (isNaN(cantidad) || cantidad === 0) {
                Swal.fire({
                    title: 'Advertencia',
                    text: 'Debe ingresar una cantidad distinta de cero',
                    icon: 'warning'
                });
                return;
            }

            $.ajax({
                url: '/Productos/ActualizarStock',
                type: 'POST',
                data: { idProducto: id, cantidad: cantidad },
                success: function (response) {
                    if (!response.esError) {
                        $('#modalStockProducto').modal('hide');
                        Productos.tabla.ajax.reload();
                        $('#CantidadStock').val('');

                        Swal.fire({
                            title: 'Éxito',
                            text: response.mensaje ?? 'Stock actualizado correctamente',
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
                        text: exceptionResponse.responseJSON?.mensaje ?? 'Error al actualizar el stock',
                        icon: 'error',
                    });
                }
            });
        },

        BuscarPorId: function () {

            const id = $('#buscarIdProducto').val();

            if (!id) {
                Swal.fire({
                    title: 'Advertencia',
                    text: 'Debe ingresar un ID',
                    icon: 'warning'
                });
                return;
            }

            $.ajax({
                url: `/Productos/ObtenerProductoPorId/${id}`,
                type: 'GET',
                success: function (response) {
                    if (response.esError || !response.data) {
                        $('#resultadoProducto').hide();
                        Swal.fire({
                            title: 'No encontrado',
                            text: 'No existe un producto con ese ID',
                            icon: 'error'
                        });
                        return;
                    }

                    const p = response.data;

                    $('#resIdProducto').text(p.id);
                    $('#resNombreProducto').text(p.nombre);
                    $('#resDescripcionProducto').text(p.descripcion);
                    $('#resPrecioProducto').text(p.precio);
                    $('#resStockProducto').text(p.stock);
                    $('#resDisponibleProducto').text(p.estaDisponible ? 'Activo' : 'Inactivo');

                    $('#resultadoProducto').show();
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

        MostrarDetallesProducto: function (id) {
            $.get(`/Productos/ObtenerProductoPorId/${id}`, function (response) {
                if (!response.esError && response.data) {
                    const p = response.data;

                    $('#detProdId').text(p.id);
                    $('#detProdNombre').text(p.nombre);
                    $('#detProdDescripcion').text(p.descripcion);
                    $('#detProdPrecio').text(p.precio);
                    $('#detProdStock').text(p.stock);
                    $('#detProdEstado').text(p.estaDisponible ? 'Activo' : 'Inactivo');

                    $('#modalDetalleProducto').modal('show');
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.mensaje ?? 'No se pudo cargar el detalle del producto',
                        icon: 'error',
                    });
                }
            }).fail(function () {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudo obtener la información del producto',
                    icon: 'error',
                });
            });
        }

    }
    $(document).ready(() => Productos.init());
})();
