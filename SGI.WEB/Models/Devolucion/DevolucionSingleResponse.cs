namespace SGI.WEB.Models.Devolucion
{
    public class DevolucionSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DevolucionEditModel Data { get; set; }
    }
}
