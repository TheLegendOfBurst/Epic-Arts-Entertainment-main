using Epic_Arts_Entertainment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Epic_Arts_Entertainment.ORM;
using System.Diagnostics;
using Epic_Arts_Entertainment.Repositorios;
using System.Text.Json;
using System.Globalization;

namespace Epic_Arts_Entertainment.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardRepositorio _dashboardRepositorio;
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(DashboardRepositorio dashboardRepositorio, ILogger<DashboardController> logger)
        {
            _dashboardRepositorio = dashboardRepositorio;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var dadosGrafico = _dashboardRepositorio.ObterDadosGrafico();
            ViewBag.ChartData = JsonSerializer.Serialize(dadosGrafico.Select(d => d.Y));
            return View();
        }

        // M�todo para contar agendamentos por ano
        public IActionResult ContarAgendamentosPorAno(int ano)
        {
            int totalAgendamentos = _dashboardRepositorio.ContarAgendamentosPorAno(ano);
            return Json(totalAgendamentos);  // Redireciona para a View Index
        }

        // M�todo para contar usu�rios por ano
        public IActionResult ContarUsuariosPorAno(int ano)
        {
            int totalUsuarios = _dashboardRepositorio.ContarUsuariosPorAno(ano);
            return Json(totalUsuarios);  // Redireciona para a View Index
        }

        // M�todo para somar o lucro por ano
        public IActionResult SomarLucroPorAno(int ano)
        {
            decimal lucroTotal = _dashboardRepositorio.SomarLucroPorAno(ano);
            return Json(lucroTotal);  // Redireciona para a View Index
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // M�todo para contar agendamentos por m�s
        public JsonResult ContarAgendamentosPorMes(int ano)
        {
            var dados = _dashboardRepositorio.ContarAgendamentosPorMes(ano);

            var categorias = dados.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Mes)).ToList();
            var valores = dados.Select(d => d.TotalAgendamentos).ToList();

            var seriesData = valores.Select(v => new { y = v }).ToList(); // Formata��o para Highcharts

            return Json(new
            {
                categorias,
                series = new[]
                {
        new { name = "Agendamentos", data = seriesData }
    }
            });
        }

        // M�todo para contar usu�rios cadastrados por m�s
        public JsonResult ContarUsuariosPorMes(int ano)
        {
            var dados = _dashboardRepositorio.ContarUsuariosPorMes(ano);

            var categorias = dados.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Mes)).ToList();
            var valores = dados.Select(d => d.TotalUsuarios).ToList();

            var seriesData = valores.Select(v => new { y = v }).ToList(); // Formata��o para Highcharts

            return Json(new
            {
                categorias,
                series = new[]
                {
        new { name = "Usu�rios Cadastrados", data = seriesData }
    }
            });
        }

        // M�todo para somar lucro por m�s
        public JsonResult SomarLucroPorMes(int ano)
        {
            var dados = _dashboardRepositorio.SomarLucroPorMes(ano);

            var categorias = dados.Select(d => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Mes)).ToList();
            var valores = dados.Select(d => d.TotalLucro).ToList();

            var seriesData = valores.Select(v => new { y = v }).ToList(); // Formata��o para Highcharts

            return Json(new
            {
                categorias,
                series = new[]
                {
        new { name = "Lucro", data = seriesData }
    }
            });
        }
    }
}