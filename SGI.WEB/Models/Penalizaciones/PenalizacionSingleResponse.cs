namespace SGI.WEB.Models.Penalizacion
{
    public class PenalizacionSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PenalizacionEditModel Data { get; set; }
    }
}
