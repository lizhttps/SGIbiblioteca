namespace SGI.WEB.Models.Penalizacion
{
    public class PenalizacionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<PenalizacionEditModel> Data { get; set; }
    }
}