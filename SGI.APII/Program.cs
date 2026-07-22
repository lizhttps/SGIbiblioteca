using Microsoft.EntityFrameworkCore;
using SGI.Persistence.context;
using SGI.IOC.Dependencies; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SigebiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SigebiDB")));

//  Registro de dependencias de la biblioteca
builder.Services.AddBibliotecaDependency();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
