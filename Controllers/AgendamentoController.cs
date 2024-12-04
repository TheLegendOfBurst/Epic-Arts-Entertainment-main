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
            // Buscar os usuários e serviços no banco de dados
            var usuarios = _usuarioRepositorio.ListarUsuarios();
            var servicos = _servicoRepositorio.ListarServicos();

            List<SelectListItem> tipoServico = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Desenvolvimento Backend .NET" },
                new SelectListItem { Value = "2", Text = "Consultoria Cloud AWS" },
                new SelectListItem { Value = "3", Text = "Implementação Kubernetes" },
                new SelectListItem { Value = "4", Text = "Segurança Cibernética" },
                new SelectListItem { Value = "5", Text = "Desenvolvimento Backend Python" },
                new SelectListItem { Value = "6", Text = "Consultoria Cloud Azure" }
            };
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


            // Buscar os agendamentos e incluir os nomes de Usuário e Serviço
            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();

            return View(agendamentos);
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

        [HttpPost]
        public IActionResult InserirAgendamento(AgendamentoVM agendamento)
        {
            try
            {
                // Log para verificar os dados recebidos
                Console.WriteLine($"Dados recebidos: {agendamento.IdUsuario}, {agendamento.IdServico}, {agendamento.DataAgendamento}, {agendamento.Horario}");

                // Verificar se os dados necessários estão presentes
                if (agendamento == null || agendamento.IdUsuario <= 0 || agendamento.IdServico <= 0)
                {
                    return Json(new { success = false, message = "Dados do agendamento inválidos." });
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

                // Chamar o método de inserção do repositório
                _agendamentoRepositorio.Inserir(novoAgendamento);

                // Retornar sucesso
                return Json(new { success = true, message = "Agendamento inserido com sucesso!" });
            }
            catch (Exception ex)
            {
                // Retornar erro em caso de exceção
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}