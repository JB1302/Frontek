$(document).ready(function () {

    document.getElementById('fechaActual').textContent =
        new Date().toLocaleDateString('es-CR', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });

    cargarStats();
    cargarOrdenes();
    cargarStockBajo();
});

function ajaxSeguro(url, callback, onError) {
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        timeout: 8000,
        success: function (data) {

            
            if (data && data.error) {
                console.error("Error backend:", data);
                if (onError) onError(data);
                return;
            }

            callback(data);
        },
        error: function (err) {
            console.error("Error AJAX:", err);
            if (onError) onError(err);
        }
    });
}

function colones(n) {
    return '\u20a1' + Number(n).toLocaleString('es-CR');
}

function badgeEstado(estado) {
    var clases = {
        'Pendiente': 'badge-pendiente',
        'Procesando':'badge-procesando',
        'En camino': 'badge-encamino',
        'Entregada': 'badge-entregada'
    };
    var c = clases[estado] || 'badge-pendiente';
    return '<span class="badge ' + c + '">' + estado + '</span>';
}

function cargarStats() {
    ajaxSeguro('/Dashboard/GetStats', function (d) {
        document.getElementById('kpiUsuarios').textContent = d.totalUsuarios;
        document.getElementById('kpiActivos').textContent  = d.usuariosActivos + ' activos';
        document.getElementById('kpiProductos').textContent = d.totalProductos;
        document.getElementById('kpiStockBajo').textContent = d.productosBajoStock + ' con stock bajo';
        document.getElementById('kpiVentas').textContent = colones(d.ventasTotal);
        document.getElementById('kpiOrdenesLabel').textContent = 'en ' + d.totalOrdenes + ' ordenes';
        document.getElementById('kpiOrdenes').textContent = d.totalOrdenes;
        document.getElementById('kpiOrdenesDetalle').textContent =
            d.ordenesPendientes + ' pendientes, ' + d.ordenesEnCamino + ' en camino';
        document.getElementById('kpiEntregadas').textContent = d.ordenesEntregadas;
        document.getElementById('kpiResenas').textContent = d.resenasPendientes;
        document.getElementById('kpiResenasTotal').textContent = d.totalResenas + ' en total';
    }, function () {
        document.getElementById('kpiUsuarios').innerHTML =
            '<small class="text-danger">Error al cargar</small>';
    });
}

function cargarOrdenes() {
    ajaxSeguro('/Dashboard/GetUltimasOrdenes', function (ordenes) {

        console.log("Respuesta ordenes:", ordenes); // 👈 debug

       
        if (!Array.isArray(ordenes)) {
            document.getElementById('ordenesBody').innerHTML =
                '<tr><td colspan="4" class="text-center text-danger py-3">Error al cargar datos</td></tr>';
            return;
        }

        // ✔️ SIN DATOS
        if (ordenes.length === 0) {
            document.getElementById('ordenesBody').innerHTML =
                '<tr><td colspan="4" class="text-center text-muted py-3">Sin ordenes registradas</td></tr>';
            return;
        }

        // ✔️ CON DATOS
        var html = '';
        ordenes.forEach(function (o) {

            html += '<tr>' +
                '<td><small class="fw-semibold">' + (o.numeroOrden || '-') + '</small></td>' +
                '<td>' + (o.nombreCliente || '-') + '</td>' +
                '<td>' + colones(o.total || 0) + '</td>' +
                '<td>' + badgeEstado(o.estado || 'Pendiente') + '</td>' +
                '</tr>';
        });

        document.getElementById('ordenesBody').innerHTML = html;

    }, function () {
        document.getElementById('ordenesBody').innerHTML =
            '<tr><td colspan="4" class="text-center text-danger py-3">No se pudo cargar</td></tr>';
    });
}
function cargarStockBajo() {
    ajaxSeguro('/Dashboard/GetStockBajo', function (productos) {

        console.log("StockBajo:", productos);

       
        if (!Array.isArray(productos)) {
            document.getElementById('stockContainer').innerHTML =
                '<p class="text-danger mb-0">Error al cargar datos</p>';
            return;
        }

        if (productos.length === 0) {
            document.getElementById('stockContainer').innerHTML =
                '<p class="text-success mb-0">Sin productos con stock critico.</p>';
            return;
        }

        var html = '';
        productos.forEach(function (p) {
            var pct = Math.max(5, Math.round((p.stock / 5) * 100));
            var color = p.stock <= 2 ? 'bg-danger' : 'bg-warning';

            html += '<div class="mb-3">' +
                '<div class="d-flex justify-content-between mb-1">' +
                '<small class="fw-semibold">' + p.nombreProducto + '</small>' +
                '<small class="text-muted">' + p.stock + ' uds</small>' +
                '</div>' +
                '<div class="progress" style="height:8px">' +
                '<div class="progress-bar ' + color + '" style="width:' + pct + '%"></div>' +
                '</div>' +
                '</div>';
        });

        document.getElementById('stockContainer').innerHTML = html;

    }, function () {
        document.getElementById('stockContainer').innerHTML =
            '<p class="text-danger mb-0">No se pudo cargar el inventario</p>';
    });
}
