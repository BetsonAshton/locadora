using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Presentation.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        public IActionResult MinhaConta()
        {
            return View();
        }
    }
}
