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


        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Cliente()
        {
            return View();
        }

        public IActionResult Admin()
        {
            // Buscar os usu�rios e servi�os no banco de dados
            var usuarios = _usuarioRepositorio.ListarUsuarios();
            var servicos = _servicoRepositorio.ListarServicos();

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

        [HttpPost]
        public IActionResult InserirAgendamento(AgendamentoVM agendamento)
        {
            try
            {
                // Log para verificar os dados recebidos
                Console.WriteLine($"Dados recebidos: {agendamento.IdUsuario}, {agendamento.IdServico}, {agendamento.DataAgendamento}, {agendamento.Horario}");

                // Verificar se os dados necess�rios est�o presentes
                if (agendamento == null || agendamento.IdUsuario <= 0 || agendamento.IdServico <= 0)
                {
                    return Json(new { success = false, message = "Dados do agendamento inv�lidos." });
                }

                // Mapear a ViewModel para a entidade da ORM (TbAgendamento)
                var novoAgendamento = new TbAgendamento
                {
                    DtHoraAgendamento = agendamento.DtHoraAgendamento,
                    DataAgendamento = agendamento.DataAgendamento,
                    Horario = agendamento.Horario,
                    IdUsuario = agendamento.IdUsuario,
                    IdServico = agendamento.IdServico
                };

                // Chamar o m�todo de inser��o do reposit�rio
                _agendamentoRepositorio.Inserir(novoAgendamento);

                // Retornar sucesso
                return Json(new { success = true, message = "Agendamento inserido com sucesso!" });
            }
            catch (Exception ex)
            {
                // Retornar erro em caso de exce��o
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}