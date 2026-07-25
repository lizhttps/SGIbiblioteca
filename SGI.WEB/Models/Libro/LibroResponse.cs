
namespace SGI.WEB.Models.Libro
{
    public class LibroResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<LibroEditModel> Data { get; set; }
    }
}
