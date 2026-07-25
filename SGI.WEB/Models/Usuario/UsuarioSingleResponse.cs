namespace SGI.WEB.Models.Usuario
{
    public class UsuarioSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UsuarioEditModel Data { get; set; } 
    }
}
