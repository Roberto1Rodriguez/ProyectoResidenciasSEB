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

// Servir archivos estáticos.
app.UseStaticFiles();

// Configurar enrutamiento.
app.UseRouting();

// Middleware de autorización (si es necesario).
app.UseAuthorization();

// Redirigir la página raíz a Login.
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    // Redirigir la raíz al Login.
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Login");
        return Task.CompletedTask;
    });
});

// Ejecutar la aplicación.
app.Run();