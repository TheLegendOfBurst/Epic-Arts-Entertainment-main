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

        private BdEpicArtsEntertainmentContext _context;

        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio, BdEpicArtsEntertainmentContext context)
        {
            _agendamentoRepositorio = agendamentoRepositorio;

            _context = context;
        }

        public IActionResult Admin()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.IdServico.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            // Chama o m�todo ListarNomesAgendamentos para obter a lista de usu�rios
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();

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


            var listaHorario = new List<SelectListItem>
  {
      new SelectListItem { Value = "8", Text = "08:00:00" },
      new SelectListItem { Value = "10", Text = "10:00:00" },
      new SelectListItem { Value = "13", Text = "13:00:00" },
      new SelectListItem { Value = "15", Text = "15:00:00" },
      new SelectListItem { Value = "17", Text = "17:00:00" },
      new SelectListItem { Value = "19", Text = "19:00:00" }
  };

            ViewBag.lstHorarios = listaHorario;

            var atendimentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(atendimentos);
        }

        public IActionResult InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int IdUsuario, int IdServico)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAgendamento, horario, IdUsuario, IdServico);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult InserirAgendamentoCliente(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            string id = Environment.GetEnvironmentVariable("USUARIO_ID");
            int IdUsuario = Int32.Parse(id);
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAtendimento, horario, IdUsuario, fkServicoId);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {

            var rs = _agendamentoRepositorio.AlterarAgendamento(id, data, servico, horario);
            if (rs)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        public IActionResult ExcluirAgendamento(int id)
        {

            var rs = _agendamentoRepositorio.ExcluirAgendamento(id);
            return Json(new { success = rs });

        }

        public IActionResult Cadastro()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.IdServico.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

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