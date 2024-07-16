async function restablecerContrasena(email) {
    try {

        var data = {
            email: email
        };

        var response = await fetch("https://localhost:7162/api/login/restablecer", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });


        if (response.ok) {

            window.location.replace("/index");
        } else {
            // Otro código de estado, mostrar mensaje de error
            errorMessage.textContent = "Error: " + await response.text();
        }
    } catch (error) {
        // Manejar errores de red o excepciones
        console.error("Error en la solicitud:", error);
    }
}