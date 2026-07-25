namespace SGI.WEB.Models.Prestamo
{
    public class PrestamoResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<PrestamoEditModel> Data { get; set; }
    }
}