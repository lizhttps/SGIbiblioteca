using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGIbiblioteca.Model.Models
{
    public class PrestamoDetalle
    {
        public int PrestamoId { get; set; }
        public string NombreUsuario { get; set; }
        public string TituloLibro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; }
    }
}