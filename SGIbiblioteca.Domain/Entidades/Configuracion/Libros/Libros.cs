using System;
using SGIbiblioteca.Domain.Base;

namespace SGIbiblioteca.Domain.Entidades.Configuracion.Libros
{

    public class Libro : AuditEntity
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Categoria { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadDisponible { get; set; }
        public string Estado { get; set; }
    }