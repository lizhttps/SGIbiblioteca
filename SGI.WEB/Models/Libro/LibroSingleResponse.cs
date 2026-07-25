namespace SGI.WEB.Models.Libro
{
    public class LibroSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public LibroEditModel Data { get; set; }
    }
}
