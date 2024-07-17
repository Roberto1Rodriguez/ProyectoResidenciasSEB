document.addEventListener('DOMContentLoaded', function () {
    const settingsIcon = document.getElementById('settingsIcon');
    const contextMenu = document.getElementById('contextMenu');
    const modal = document.getElementById('cambiarContrasenaModal');
    const closeModal = document.querySelector('.modal .close');
    const cambiarContrasenaForm = document.getElementById('cambiarContrasenaForm');

    settingsIcon.addEventListener('click', function (event) {
        event.stopPropagation();
        contextMenu.style.display = 'block';
        const rect = settingsIcon.getBoundingClientRect();
        contextMenu.style.top = `${rect.bottom + window.scrollY}px`;
        contextMenu.style.left = `${rect.left + window.scrollX}px`;
    });

    document.addEventListener('click', function () {
        contextMenu.style.display = 'none';
    });

    contextMenu.addEventListener('click', function (event) {
        event.stopPropagation();
    });

    contextMenu.addEventListener('click', function (event) {
        if (event.target.id === 'cambiar-contraseña') {
            modal.style.display = 'block';
            contextMenu.style.display = 'none';
        } else if (event.target.id === 'logout') {
            sessionStorage.clear();
            window.location.href = 'login';
        }
    });

    closeModal.onclick = function () {
        modal.style.display = 'none';
    };

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = 'none';
        }
    };

    cambiarContrasenaForm.addEventListener('submit', function (event) {
        event.preventDefault();

        const currentPassword = document.getElementById('current-password').value;
        const newPassword = document.getElementById('new-password').value;
        const confirmNewPassword = document.getElementById('confirm-new-password').value;
        const usuarioId = sessionStorage.getItem('usuarioId'); // Obtener el ID de usuario de sessionStorage

        if (newPassword !== confirmNewPassword) {
            alert('Las nuevas contraseñas no coinciden');
            return;
        }

        fetch('https://localhost:7162/api/login/cambiarcontrasena', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: usuarioId,
                ContrasenaActual: currentPassword,
                ContrasenaNueva: newPassword
            })
        }).then(response => {
            if (response.ok) {
                alert('Contraseña cambiada con éxito');
                modal.style.display = 'none';
            } else {
                alert('Error al cambiar la contraseña');
            }
        }).catch(error => {
            console.error('Error al cambiar la contraseña:', error);
        });
    });
});