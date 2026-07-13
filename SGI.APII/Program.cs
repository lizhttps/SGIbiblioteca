using Microsoft.EntityFrameworkCore;
using SGI.Persistence.context;
using SGI.IOC.Dependencies; 

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Registro de tu DbContext
builder.Services.AddDbContext<SigebiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SigebiDB")));

// 3. Registro de todas tus dependencias de la biblioteca
builder.Services.AddBibliotecaDependency();

// 4. Construir la aplicación
var app = builder.Build();

// 5. Configurar el pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
