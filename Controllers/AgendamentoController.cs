using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using Epic_Arts_Entertainment.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Epic_Arts_Entertainment.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;
        private readonly UsuarioRepositorio _usuarioRepositorio;
        private readonly ServicoRepositorio _servicoRepositorio;

        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio, UsuarioRepositorio usuarioRepositorio, ServicoRepositorio servicoRepositorio)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _servicoRepositorio = servicoRepositorio;
        }

        public IActionResult Admin()
        {
            // Buscar os usu�rios e servi�os no banco de dados
            var usuarios = _usuarioRepositorio.ListarUsuarios();
            var servicos = _servicoRepositorio.ListarServicos();

            List<SelectListItem> tipoServico = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Desenvolvimento Backend .NET" },
                new SelectListItem { Value = "2", Text = "Consultoria Cloud AWS" },
                new SelectListItem { Value = "3", Text = "Implementa��o Kubernetes" },
                new SelectListItem { Value = "4", Text = "Seguran�a Cibern�tica" },
                new SelectListItem { Value = "5", Text = "Desenvolvimento Backend Python" },
                new SelectListItem { Value = "6", Text = "Consultoria Cloud Azure" }
            };

            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.IdUsuario.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.Nome             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            // Passar a lista para a View usando ViewBag
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");

            // Preencher as listas para os dropdowns
            List<SelectListItem> idUsuario = usuarios.Select(u => new SelectListItem
            {
                Value = u.IdUsuario.ToString(),
                Text = u.Nome
            }).ToList();

            List<SelectListItem> idServico = servicos.Select(s => new SelectListItem
            {
                Value = s.IdServico.ToString(),
                Text = s.TipoServico
            }).ToList();

            ViewBag.lstIdUsuario = new SelectList(idUsuario, "Value", "Text");
            ViewBag.lstIdServico = new SelectList(idServico, "Value", "Text");

            // Buscar os agendamentos e incluir os nomes de Usu�rio e Servi�o
            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();

            return View(agendamentos);
        }

        public IActionResult Cadastro()
        {
            return View();
        }


        public IActionResult Create()
        {
            // Recarrega as listas de usu�rios e servi�os
            var usuarios = _usuarioRepositorio.ListarUsuarios();
            var servicos = _servicoRepositorio.ListarServicos();

            List<SelectListItem> idUsuario = usuarios.Select(u => new SelectListItem
            {
                Value = u.IdUsuario.ToString(),
                Text = u.Nome
            }).ToList();

            List<SelectListItem> idServico = servicos.Select(s => new SelectListItem
            {
                Value = s.IdServico.ToString(),
                Text = s.TipoServico
            }).ToList();

            ViewBag.lstIdUsuario = new SelectList(idUsuario, "Value", "Text");
            ViewBag.lstIdServico = new SelectList(idServico, "Value", "Text");

            return View();
        }

        public IActionResult ConsultarAgendamento(string data)
        {

            var agendamento = _agendamentoRepositorio.ConsultarAgendamento(data);

            if (agendamento != null)
            {
                return Json(agendamento);
            }
            else
            {
                return NotFound();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}