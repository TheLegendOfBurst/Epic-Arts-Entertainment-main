using Epic_Arts_Entertainment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Epic_Arts_Entertainment.ORM;
using System.Diagnostics;
using Epic_Arts_Entertainment.Repositorios;

namespace Epic_Arts_Entertainment.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<UsuarioController> _logger;
        public RelatorioController(UsuarioRepositorio usuarioRepositorio, ILogger<UsuarioController> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}