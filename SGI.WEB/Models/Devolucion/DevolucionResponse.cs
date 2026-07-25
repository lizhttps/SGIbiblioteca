namespace SGI.WEB.Models.Devolucion
{
    public class DevolucionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DevolucionEditModel> Data { get; set; }
    }
}