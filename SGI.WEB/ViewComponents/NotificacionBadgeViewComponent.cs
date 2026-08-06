using Microsoft.AspNetCore.Mvc;
using SGI.WEB.Services.Notificacion;
using System.Security.Claims;

namespace SGI.WEB.ViewComponents
{
    public class NotificacionBadgeViewComponent : ViewComponent
    {
        private readonly INotificacionApiService _notificacionApiService;

        public NotificacionBadgeViewComponent(INotificacionApiService notificacionApiService)
        {
            _notificacionApiService = notificacionApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int unreadCount = 0;

            try
            {
                var claimsPrincipal = User as System.Security.Claims.ClaimsPrincipal;
                var userIdClaim = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userId = int.Parse(userIdClaim.Value);
                    var response = await _notificacionApiService.GetNotificacionesByUsuario(userId);
                    unreadCount = response?.Data?.Count(n => !n.Leido) ?? 0;
                }
            }
            catch
            {
                unreadCount = 0;
            }

            return View(unreadCount);
        }
    }
}