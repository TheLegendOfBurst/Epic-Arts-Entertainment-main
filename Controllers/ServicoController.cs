using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Epic_Arts_Entertainment.Controllers
{
    public class ServicoController : Controller
    {
        private readonly ServicoRepositorio _servicoRepositorio;
        private readonly ILogger<ServicoController> _logger;

        public ServicoController(ServicoRepositorio servicoRepositorio, ILogger<ServicoController> logger)
        {
            _servicoRepositorio = servicoRepositorio;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Chama o método ListarNomesAgendamentos para obter a lista de usuários
            var nomeServicos = _servicoRepositorio.ListarNomesServicos();

            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.IdServico.ToString(),  // O valor do item será o ID do usuário
                    Text = u.TipoServico             // O texto exibido será o nome do usuário
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Servicos = selectList;
            }

            var Servicos = _servicoRepositorio.ListarServicos();
            return View(Servicos);
        }

        public IActionResult InserirServico(string tipoServico, decimal valor)
        {
            try
            {
                // Mapeamento dos tipos de serviço (podemos fazer isso diretamente ou usar um serviço)
                var tipoServicoMap = new Dictionary<string, string>
        {
            { "0", "Desenvolvimento de Jogos" },
            { "1", "Criação de Roteiro e Narrativas" },
            { "2", "Design De Personagens e Ambientes" },
            { "3", "Marketing e Lançamento de Jogos" },
            { "4", "Teste e Garantia de Qualidade (QA)" },
            { "5", "Suporte e Manutenção de Jogos e Consoles" }
        };

                // Obter o nome do tipo de serviço baseado no valor selecionado
                string nomeTipoServico = tipoServicoMap[tipoServico];

                // Chama o método do repositório que realiza a inserção no banco de dados
                var resultado = _servicoRepositorio.InserirServico(nomeTipoServico, valor);

                // Verifica o resultado da inserção
                if (resultado)
                {
                    // Se o resultado for verdadeiro, significa que o serviço foi inserido com sucesso
                    return Json(new { success = true, message = "Serviço inserido com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, significa que houve um erro ao tentar inserir o serviço
                    return Json(new { success = false, message = "Erro ao inserir o serviço. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }

        public IActionResult AtualizarServico(int id, string tipoServico, decimal valor)
        {
            try
            {
                // Chama o repositório para atualizar o serviço
                var resultado = _servicoRepositorio.AtualizarServico(id, tipoServico, valor);

                if (resultado)
                {
                    return Json(new { success = true, message = "Serviço atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o serviço. Verifique se o serviço existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }

        public IActionResult ExcluirServico(int id)
        {
            try
            {
                // Chama o repositório para excluir o serviço
                var resultado = _servicoRepositorio.ExcluirServico(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Serviço excluído com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Não foi possível excluir o serviço. Verifique se ele está vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
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