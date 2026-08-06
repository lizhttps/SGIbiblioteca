namespace SGI.Application.Dtos.Prestamo
{
    public class PrestamoDecisionDto
    {
        public int Id { get; set; }
        public int UsuarioMod { get; set; } // el bibliotecario que aprueba/rechaza

        public DateTime? FechaDevolucion { get; set; }
    }
}