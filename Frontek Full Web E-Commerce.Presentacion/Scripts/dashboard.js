$(document).ready(function () {

    document.getElementById('fechaActual').textContent =
        new Date().toLocaleDateString('es-CR', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });

    // Evita 3 requests simultáneos contra el mismo contexto EF.
    cargarStats(function () {
        cargarOrdenes(function () {
            cargarStockBajo();
        });
    });
});

function ajaxSeguro(url, callback, onError) {
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        timeout: 8000,
        success: function (data) {
            var payload = data && data.d ? data.d : data;

            if (payload && payload.error) {
                console.error("Error backend:", payload);
                if (onError) onError(payload);
                return;
            }

            callback(payload);
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
        'Procesando': 'badge-procesando',
        'En camino': 'badge-encamino',
        'Entregada': 'badge-entregada'
    };
    var c = clases[estado] || 'badge-pendiente';
    return '<span class="badge ' + c + '">' + estado + '</span>';
}

function cargarStats(done) {
    ajaxSeguro('/Dashboard/GetStats', function (d) {
        document.getElementById('kpiUsuarios').textContent = d.totalUsuarios;
        document.getElementById('kpiActivos').textContent = d.usuariosActivos + ' activos';
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
        if (done) done();
    }, function () {
        document.getElementById('kpiUsuarios').innerHTML =
            '<small class="text-danger">Error al cargar</small>';
        if (done) done();
    });
}

function cargarOrdenes(done) {
    ajaxSeguro('/Dashboard/GetUltimasOrdenes', function (ordenes) {

        if (!Array.isArray(ordenes)) {
            document.getElementById('ordenesBody').innerHTML =
                '<tr><td colspan="4" class="text-center text-danger py-3">Error al cargar datos</td></tr>';
            if (done) done();
            return;
        }

        // ✔️ SIN DATOS
        if (ordenes.length === 0) {
            document.getElementById('ordenesBody').innerHTML =
                '<tr><td colspan="4" class="text-center text-muted py-3">Sin ordenes registradas</td></tr>';
            if (done) done();
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
        if (done) done();

    }, function (err) {
        var mensaje = (err && err.mensaje) ? err.mensaje : 'No se pudo cargar';
        document.getElementById('ordenesBody').innerHTML =
            '<tr><td colspan="4" class="text-center text-danger py-3">' + mensaje + '</td></tr>';
        if (done) done();
    });
}
function cargarStockBajo() {
    ajaxSeguro('/Dashboard/GetStockBajo', function (productos) {
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

    }, function (err) {
        var mensaje = (err && err.mensaje) ? err.mensaje : 'No se pudo cargar el inventario';
        document.getElementById('stockContainer').innerHTML =
            '<p class="text-danger mb-0">' + mensaje + '</p>';
    });
}