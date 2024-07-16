document.addEventListener('DOMContentLoaded', function () {
    const settingsIcon = document.getElementById('settingsIcon');
    const contextMenu = document.getElementById('contextMenu');

    settingsIcon.addEventListener('click', function (event) {
        event.stopPropagation();
        contextMenu.style.display = 'block';
        const rect = icon.getBoundingClientRect();
        contextMenu.style.top = `${rect.bottom + window.scrollY}px`;
        contextMenu.style.left = `${rect.left + window.scrollX}px`;
    });

    document.addEventListener('click', function () {
        contextMenu.style.display = 'none';
    });

    contextMenu.addEventListener('click', function (event) {
        event.stopPropagation();
    });
});


// Obtener el modal
var modal = document.getElementById("myModal");

// Obtener el botón que abre el modal
var btn = document.getElementById("openModalBtn");

// Obtener el elemento <span> que cierra el modal
var span = document.getElementsByClassName("close")[0];

// Cuando el usuario hace clic en el botón, se abre el modal
//btn.onclick = function() {
    //modal.style.display = "block";
//}
//

// Cuando el usuario hace clic en <span> (x), se cierra el modal
//span.onclick = function() {
  //  modal.style.display = "none";
//}

// Cuando el usuario hace clic en cualquier lugar fuera del modal, se cierra
window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const icon = document.getElementById('icon');
    const contextMenu = document.getElementById('contextMenu');

    icon.addEventListener('click', function (event) {
        event.stopPropagation();
        contextMenu.style.display = 'block';
    });

    document.addEventListener('click', function (event) {
        if (event.target !== contextMenu && event.target !== icon) {
            contextMenu.style.display = 'none';
        }
    });

    contextMenu.addEventListener('click', function (event) {
        if (event.target.id === 'logout') {
            alert('Cerrando sesión...');
            // Aquí puedes agregar la lógica para cerrar sesión
        }
    });
});

