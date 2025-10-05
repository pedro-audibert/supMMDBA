/*
================================================================================================
ARQUIVO: HomeController.cs
FUN��O:  Controlador principal MVC (Model-View-Controller) respons�vel por servir as
         p�ginas (Views) da aplica��o. Funcionalidades principais:
         1. Servir a p�gina 'Index' como portal de sele��o de m�quinas.
         2. Servir as Views de pain�is de m�quinas (Rotuladora, Enchedora, etc.).
         3. Servir p�ginas p�blicas e utilit�rias (Privacidade, Erro).
================================================================================================
*/

#region NAMESPACES
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mmdba.Models;
using System.Diagnostics;
#endregion

namespace mmdba.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// P�gina principal do portal de sele��o de m�quinas.
        /// </summary>
        public IActionResult Index()
        {
            _logger.LogInformation("Usu�rio '{UserName}' acessou o portal de sele��o de m�quinas.", User.Identity?.Name);
            return View();
        }

        /// <summary>
        /// Painel de supervis�o em tempo real.
        /// </summary>
        public IActionResult PainelSupervisao()
        {
            _logger.LogInformation("Usu�rio '{UserName}' acessou o Painel de Supervis�o.", User.Identity?.Name);
            return View();
        }

        /// <summary>
        /// Painel de manuten��o da Rotuladora BOPP.
        /// </summary>
        public IActionResult PainelManutencao()
        {
            _logger.LogInformation("Usu�rio '{UserName}' acessou o Painel de Manuten��o da Rotuladora BOPP.", User.Identity?.Name);
            return View();
        }

        /// <summary>
        /// Painel de alarmes da Enchedora.
        /// </summary>
        public IActionResult PainelAlarmes()
        {
            _logger.LogInformation("Usu�rio '{UserName}' acessou o Painel de Alarmes da Enchedora.", User.Identity?.Name);
            return View();
        }

        /// <summary>
        /// P�gina de pol�tica de privacidade (p�blica).
        /// </summary>
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            _logger.LogInformation("Visitante acessou a p�gina de Privacidade.");
            return View();
        }

        /// <summary>
        /// P�gina de erro gen�rica (p�blica).
        /// </summary>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("P�gina de erro exibida. RequestId: {RequestId}", requestId);
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
