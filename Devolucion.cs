using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entities.Devolucion
{
    public class Devolucion : AuditEntity
{
    public int PrestamoId { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public bool DevueltoATiempo { get; set; }
    public string Observaciones { get; set; }
}