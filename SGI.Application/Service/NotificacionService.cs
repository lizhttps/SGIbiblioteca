using SGI.Application.Dtos.Notificacion;
using SGI.Application.Interfaces;
using SGIbiblioteca.Domain.Base;
using SGIbiblioteca.Domain.Entidades.Configuracion.Notificaciones;
using SGIbiblioteca.Domain.Repositorio;

namespace SGI.Application.Service
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _NotificacionRepository;
        private readonly ILoggerService _logger;

        public NotificacionService(INotificacionRepository NotificacionRepository, ILoggerService logger)
        {
            _NotificacionRepository = NotificacionRepository;
            _logger = logger;
        }

        public async Task<OperationResult> GetData()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _NotificacionRepository.GetAllAsync())
                    .Select(Noti => new NotificacionUpdateDto()
                    {
                        Id = Noti.Id,
                        UsuarioId = Noti.UsuarioId,
                        Mensaje = Noti.Mensaje,
                        Leido = Noti.Leido,
                        FechaMod = Noti.FechaCreacion,
                        UsuarioMod = int.TryParse(Noti.CreadoPor, out int user) ? user : 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo la Notificacion";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> GetDataById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var notiID = await _NotificacionRepository.GetEntityByIdAsync(id);

                if (notiID == null)
                {
                    result.Success = false;
                    result.Message = "Notificación no encontrada.";
                    return result;
                }

                var notiresult = new NotificacionUpdateDto()
                {
                    Id = notiID.Id,
                    UsuarioId = notiID.UsuarioId,
                    Mensaje = notiID.Mensaje,
                    Leido = notiID.Leido,
                    FechaMod = notiID.FechaCreacion,
                    UsuarioMod = int.TryParse(notiID.CreadoPor, out int user) ? user : 0
                };
                result.Data = notiresult;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo la notificación.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }


        public async Task<OperationResult> GetNotiUsuario(int usuarioid)
        {
            OperationResult result = new OperationResult();
            try
            {
                var notificaciones = await _NotificacionRepository.GetByUsuarioIdAsync(usuarioid);
                var notificacionresult = notificaciones.Select(n => new NotificacionUpdateDto()
                {
                    Id = n.Id,
                    UsuarioId = n.UsuarioId,
                    Mensaje = n.Mensaje,
                    Leido = n.Leido,
                    FechaMod = n.FechaCreacion,
                    UsuarioMod = int.TryParse(n.CreadoPor, out int user) ? user : 0
                }).ToList();

                result.Data = notificacionresult;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Remove(NotificacionRemoveDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var NotificacionToDelete = await _NotificacionRepository.GetEntityByIdAsync(dto.Id);

                if (NotificacionToDelete == null) // si el Notificacion no existe, devolvemos un error.
                {
                    result.Success = false;
                    result.Message = "Notificacion no encontrada.";
                    return result;
                }
                NotificacionToDelete.Estado = dto.Estado;

                // Notificacion no tiene una entidad de estado


                await _NotificacionRepository.UpdateEntityAsync(NotificacionToDelete);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al intentar eliminar la notificacion.";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public async Task<OperationResult> Save(NotificacionSaveDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _NotificacionRepository.SaveEntityAsync(new Notificacion()
                {
                    UsuarioId = dto.UsuarioId,
                    Mensaje = dto.Mensaje,
                    FechaCreacion = dto.FechaMod,
                    CreadoPor = dto.UsuarioMod.ToString(),
                    Leido = false,
                    Estado = true,
                    FechaEnvio = dto.FechaMod

                });
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = "Error guardando la Notificacion";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async Task<OperationResult> Update(NotificacionUpdateDto dto)
        {
            OperationResult result = new OperationResult();

            try
            {
                var NotficicacionToUpdate = await _NotificacionRepository.GetEntityByIdAsync(dto.Id);
                NotficicacionToUpdate.UsuarioId = dto.UsuarioId;
                NotficicacionToUpdate.Mensaje = dto.Mensaje;
                NotficicacionToUpdate.Leido = dto.Leido;
                NotficicacionToUpdate.ModificadoPor = dto.UsuarioMod.ToString();
                NotficicacionToUpdate.FechaModificacion = dto.FechaMod;


                await _NotificacionRepository.UpdateEntityAsync(NotficicacionToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando la Notificacion";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}
