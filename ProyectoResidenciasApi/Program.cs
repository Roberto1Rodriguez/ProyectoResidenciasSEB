using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text;
using ProyectoResidenciasApi.Data;
using Microsoft.AspNetCore.Hosting.Server;
using PdfSharp.Fonts;
using ProyectoResidenciasApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Sistem21ResidenciasSebContext>(c => c.UseMySql("server=192.250.231.19;user=sistem21_residenciasSEB;password=sistem21_;database=sistem21_residenciasSEB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.7-mariadb")));

var app = builder.Build();
GlobalFontSettings.FontResolver = new CustomFontResolver();

app.UseCors(builder =>
builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseStaticFiles();
app.UseDefaultFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
