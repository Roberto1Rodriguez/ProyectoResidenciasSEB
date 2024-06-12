var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddCors();
var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins("https://localhost:7162");
    options.WithOrigins("https://localhost:7259");
});
app.MapRazorPages();
app.UseStaticFiles();
app.Run();
