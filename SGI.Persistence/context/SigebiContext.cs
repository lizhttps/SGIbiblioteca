using Microsoft.EntityFrameworkCore;
using SGIbiblioteca.Domain.Entidades.Configuracion.Libros;
using SGIbiblioteca.Domain.Entidades.Configuracion.Usuarios;
using SGIbiblioteca.Domain.Entidades.Configuracion.Prestamos;
using SGIbiblioteca.Domain.Entidades.Configuracion.Devoluciones;
using SGIbiblioteca.Domain.Entities.Penalizaciones;
using SGIbiblioteca.Domain.Entidades.Configuracion.Notificaciones;
using SGIbiblioteca.Domain.Entities.Auditorias;
namespace SGI.Persistence.context
{
    public class SigebiContext : DbContext
    {
        public SigebiContext(DbContextOptions<SigebiContext> options) : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = new List<Auditoria>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // Evita auditar la propia tabla de Auditoria (loop infinito)
                if (entry.Entity is Auditoria) continue;

                if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
                    continue;

                var accion = entry.State == EntityState.Added ? "Crear" : "Actualizar";

                // Detecta soft-delete: si Estado pasó de true a false, la acción real es "Eliminar"
                var estadoProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Estado");
                if (entry.State == EntityState.Modified && estadoProp != null &&
                    estadoProp.OriginalValue is true && estadoProp.CurrentValue is false)
                {
                    accion = "Eliminar";
                }

                var idProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id");

                var realizadoPor = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "ModificadoPor")?.CurrentValue?.ToString();
                if (string.IsNullOrWhiteSpace(realizadoPor))
                {
                    realizadoPor = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreadoPor")?.CurrentValue?.ToString();
                }
                if (string.IsNullOrWhiteSpace(realizadoPor))
                {
                    realizadoPor = "Desconocido";
                }

                auditEntries.Add(new Auditoria
                {
                    Entidad = entry.Entity.GetType().Name,
                    Accion = accion,
                    Detalle = $"{accion} de {entry.Entity.GetType().Name} con id {(idProp?.CurrentValue ?? "N/A")}",
                    EntidadId = idProp != null && idProp.CurrentValue != null ? Convert.ToInt32(idProp.CurrentValue) : 0,
                    RealizadoPor = realizadoPor,
                    FechaAccion = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    CreadoPor = realizadoPor,
                    ModificadoPor = realizadoPor,
                    Estado = true
                });
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Any())
            {
                Auditorias.AddRange(auditEntries);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}