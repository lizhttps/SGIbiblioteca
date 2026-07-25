namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PrestamoEditModel Data { get; set; }
    }
}