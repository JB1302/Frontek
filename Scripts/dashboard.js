// dashboard.js
// urlStats, urlOrdenes y urlStock vienen de Index.cshtml
// Razor las genera con @Url.Action para que no estén quemadas acá

function formatColones(n) {
    return '\u20A1' + parseFloat(n).toLocaleString('es-CR', { minimumFractionDigits: 2 });
}

function formatFecha(str) {
    var d = new Date(str);
    if (isNaN(d)) return str;
    return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
}

function colorEstado(estado) {
    if (estado === 'Entregada') return 'success';
    if (estado === 'En camino') return 'primary';
    if (estado === 'Procesando') return 'warning';
    return 'secondary';
}

function cargarEstadisticas() {
    $.getJSON(urlStats)
        .done(function (d) {
            $('#numUsuarios').text(d.totalUsuarios);
            $('#numActivos').text(d.usuariosActivos + ' activos');

            $('#numProductos').text(d.totalProductos);
            if (d.productosBajoStock > 0)
                $('#numBajoStock').text(d.productosBajoStock + ' con stock bajo');

            $('#numOrdenes').text(d.totalOrdenes);
            $('#numEntregadas').text(d.ordenesEntregadas + ' entregadas');

            $('#numVentas').text(formatColones(d.ventasTotal));

            $('#numResenas').text(d.totalResenas);
            if (d.resenasPendientes > 0)
                $('#resenasAlert').html('<span class="text-warning">Hay ' + d.resenasPendientes + ' reseña(s) pendiente(s) de revisar</span>');

            var total = d.totalOrdenes || 1;
            var estados = [
                { nombre: 'Entregadas', valor: d.ordenesEntregadas, color: '#198754' },
                { nombre: 'En camino', valor: d.ordenesEnCamino, color: '#0d6efd' },
                { nombre: 'Procesando', valor: d.ordenesProcesando, color: '#ffc107' },
                { nombre: 'Pendientes', valor: d.ordenesPendientes, color: '#6c757d' }
            ];

            var html = '';
            estados.forEach(function (e) {
                var pct = Math.round((e.valor / total) * 100);
                html += '<div class="mb-3">'
                    + '<div class="d-flex justify-content-between">'
                    + '<span>' + e.nombre + '</span>'
                    + '<span>' + e.valor + '</span>'
                    + '</div>'
                    + '<div class="barra-fondo">'
                    + '<div class="barra-relleno" style="width:' + pct + '%; background:' + e.color + ';"></div>'
                    + '</div>'
                    + '</div>';
            });
            $('#contenidoBarras').html(html);
        })
        .fail(function () {
            $('#msgError').show();
        });
}

function cargarOrdenes() {
    $.getJSON(urlOrdenes)
        .done(function (lista) {
            if (!lista.length) {
                $('#filaOrdenes').html('<tr><td colspan="5" class="text-center text-muted">No hay órdenes todavía.</td></tr>');
                return;
            }
            var html = '';
            lista.forEach(function (o) {
                html += '<tr>'
                    + '<td>' + o.numeroOrden + '</td>'
                    + '<td>' + o.nombreCliente + '</td>'
                    + '<td>' + formatColones(o.total) + '</td>'
                    + '<td><span class="badge bg-' + colorEstado(o.estado) + '">' + o.estado + '</span></td>'
                    + '<td>' + formatFecha(o.fechaCreacion) + '</td>'
                    + '</tr>';
            });
            $('#filaOrdenes').html(html);
        });
}

function cargarStock() {
    $.getJSON(urlStock)
        .done(function (lista) {
            if (!lista.length) return;

            $('#seccionStock').show();
            var html = '';
            lista.forEach(function (p) {
                var badge = p.stock === 0
                    ? '<span class="badge bg-danger">Sin stock</span>'
                    : '<span class="badge bg-warning text-dark">' + p.stock + ' uds.</span>';
                html += '<tr>'
                    + '<td>' + p.nombreProducto + '</td>'
                    + '<td>' + badge + '</td>'
                    + '<td>' + formatColones(p.precio) + '</td>'
                    + '</tr>';
            });
            $('#filaStock').html(html);
        });
}

$(document).ready(function () {
    cargarEstadisticas();
    cargarOrdenes();
    cargarStock();
});