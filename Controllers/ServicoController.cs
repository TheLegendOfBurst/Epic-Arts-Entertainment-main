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
            // Chama o m�todo ListarNomesAgendamentos para obter a lista de usu�rios
            var nomeServicos = _servicoRepositorio.ListarNomesServicos();

            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.IdServico.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
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
                // Mapeamento dos tipos de servi�o (podemos fazer isso diretamente ou usar um servi�o)
                var tipoServicoMap = new Dictionary<string, string>
        {
            { "0", "Desenvolvimento de Jogos" },
            { "1", "Cria��o de Roteiro e Narrativas" },
            { "2", "Design De Personagens e Ambientes" },
            { "3", "Marketing e Lan�amento de Jogos" },
            { "4", "Teste e Garantia de Qualidade (QA)" },
            { "5", "Suporte e Manuten��o de Jogos e Consoles" }
        };

                // Obter o nome do tipo de servi�o baseado no valor selecionado
                string nomeTipoServico = tipoServicoMap[tipoServico];

                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _servicoRepositorio.InserirServico(nomeTipoServico, valor);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    // Se o resultado for verdadeiro, significa que o servi�o foi inserido com sucesso
                    return Json(new { success = true, message = "Servi�o inserido com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, significa que houve um erro ao tentar inserir o servi�o
                    return Json(new { success = false, message = "Erro ao inserir o servi�o. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult AtualizarServico(int id, string tipoServico, decimal valor)
        {
            try
            {
                // Chama o reposit�rio para atualizar o servi�o
                var resultado = _servicoRepositorio.AtualizarServico(id, tipoServico, valor);

                if (resultado)
                {
                    return Json(new { success = true, message = "Servi�o atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o servi�o. Verifique se o servi�o existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult ExcluirServico(int id)
        {
            try
            {
                // Chama o reposit�rio para excluir o servi�o
                var resultado = _servicoRepositorio.ExcluirServico(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Servi�o exclu�do com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "N�o foi poss�vel excluir o servi�o. Verifique se ele est� vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
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