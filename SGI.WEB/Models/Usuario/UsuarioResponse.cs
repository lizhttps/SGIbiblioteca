namespace SGI.WEB.Models.Usuario
{
    public class UsuarioResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<UsuarioEditModel> Data { get; set; }
    }
}