var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddRazorPages();
builder.Services.AddCors();

var app = builder.Build();

// Configurar CORS.
app.UseCors(options =>
{
    options.WithOrigins("https://localhost:7162", "https://localhost:7259")
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Servir archivos est�ticos.
app.UseStaticFiles();

// Configurar enrutamiento.
app.UseRouting();

// Middleware de autorizaci�n (si es necesario).
app.UseAuthorization();

// Redirigir la p�gina ra�z a Login.
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    // Redirigir la ra�z al Login.
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Login");
        return Task.CompletedTask;
    });
});

// Ejecutar la aplicaci�n.
app.Run();