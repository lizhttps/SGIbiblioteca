using SGI.Application.Base;

public class PrestamoSaveDto : DtoBase
{
    public int LibroId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaLimite { get; set; }
}