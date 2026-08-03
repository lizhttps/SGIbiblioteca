using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SGIbiblioteca.Domain.Interfaces;
using SGI.Application.Dtos.Auth;
using SGI.WEB.Models.Auth;
using SGI.WEB.Services;
using System.Security.Claims;

namespace SGI.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiService _authApiService;
        private readonly ILoggerService _loggerService;

        public AccountController(IAuthApiService authApiService, ILoggerService loggerService)
        {
            _authApiService = authApiService;
            _loggerService = loggerService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var dto = new UsuarioLoginDto { Correo = model.Correo, Password = model.Password };
                var response = await _authApiService.LoginAsync(dto);

                if (!response.Success || response.Data == null)
                {
                    ModelState.AddModelError(string.Empty, response.Message ?? "Correo o contraseña incorrectos.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.Id.ToString()),
                    new Claim(ClaimTypes.Email, response.Data.Correo),
                    new Claim(ClaimTypes.Name, response.Data.Nombre),
                    new Claim(ClaimTypes.Role, response.Data.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al intentar iniciar sesión con el correo: {model.Correo}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var dto = new UsuarioRegisterDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Correo = model.Correo,
                    Password = model.Password,
                    Rol = model.Rol
                };

                var response = await _authApiService.RegisterAsync(dto);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message ?? "No se pudo completar el registro.");
                    return View(model);
                }

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, $"Error al registrar el usuario con correo: {model.Correo}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            return View();
        }
    }
}