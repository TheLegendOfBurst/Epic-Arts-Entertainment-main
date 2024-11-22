using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Epic_Arts_Entertainment.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ILogger<AgendamentoController> _logger;
        private readonly AgendamentoRepositorio _agendamentoRepositorio;

        // Construtor com inje��o de depend�ncia do reposit�rio
        public AgendamentoController(ILogger<AgendamentoController> logger, AgendamentoRepositorio agendamentoRepositorio)
        {
            _logger = logger;
            _agendamentoRepositorio = agendamentoRepositorio;
        }

        // P�gina Admin (listar todos os agendamentos)
        public IActionResult Admin()
        {
            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(agendamentos);
        }

        // P�gina Cadastro (formul�rio para novo agendamento)
        public IActionResult Cadastro()
        {
            return View();
        }

        // A��o para criar um agendamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int idUsuario, int idServico)
        {
            var sucesso = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAgendamento, horario, idUsuario, idServico);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Agendamento criado com sucesso!";
                return RedirectToAction("Admin");
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao criar agendamento.";
                return View();
            }
        }

        // P�gina Cliente (listar agendamentos do cliente)
        public IActionResult Cliente()
        {
            return View();
        }

        // A��o para excluir um agendamento
        public IActionResult Excluir(int id)
        {
            var sucesso = _agendamentoRepositorio.ExcluirAgendamento(id);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Agendamento exclu�do com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao excluir o agendamento.";
            }

            return RedirectToAction("Admin");
        }

        // A��o para editar um agendamento
        public IActionResult Editar(int id)
        {
            var agendamento = _agendamentoRepositorio.ListarAgendamentos().FirstOrDefault(a => a.IdAgendamento == id);
            if (agendamento == null)
            {
                return NotFound();
            }
            return View(agendamento);
        }

        // A��o para atualizar o agendamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int idUsuario, int idServico)
        {
            var sucesso = _agendamentoRepositorio.AtualizarAgendamento(id, dtHoraAgendamento, dataAgendamento, horario, idUsuario, idServico);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Agendamento atualizado com sucesso!";
                return RedirectToAction("Admin");
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao atualizar o agendamento.";
                return View();
            }
        }

        // A��o para exibir erros
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
