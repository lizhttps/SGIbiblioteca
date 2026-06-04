using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entidades.Configuracion.NewFolder1
{
    public class Devolucion : AuditEntity
{
    public int PrestamoId { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public bool DevueltoATiempo { get; set; }
    public string Observaciones { get; set; }
}