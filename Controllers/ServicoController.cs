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
            // Lista de tipos de serviço
            List<SelectListItem> tipoServico = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Desenvolvimento de Jogos" },
        new SelectListItem { Value = "1", Text = "Criação de Roteiro e Narrativas" },
        new SelectListItem { Value = "2", Text = "Design De Personagens e Ambientes" },
        new SelectListItem { Value = "3", Text = "Marketing e Lançamento de Jogos" },
        new SelectListItem { Value = "4", Text = "Teste e Garantia de Qualidade (QA)" },
        new SelectListItem { Value = "5", Text = "Suporte e Manutenção de Jogos e Consoles" }
    };

            // Lista de valores fixos para o serviço
            List<SelectListItem> valoresServico = new List<SelectListItem>
    {
        new SelectListItem { Value = "100", Text = "R$ 100" },
        new SelectListItem { Value = "200", Text = "R$ 200" },
        new SelectListItem { Value = "300", Text = "R$ 300" },
        new SelectListItem { Value = "500", Text = "R$ 500" }
    };

            // Passando as listas para a View
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");
            ViewBag.lstValoresServico = new SelectList(valoresServico, "Value", "Text");

            // Carregar os serviços já existentes
            var servicos = _servicoRepositorio.ListarServicos();

            // Mapeamento do tipo de serviço para o nome
            var tiposServicoMap = tipoServico.ToDictionary(t => t.Value, t => t.Text);

            // Passar os serviços e o mapeamento de tipos para a view
            ViewBag.TiposServicoMap = tiposServicoMap;
            return View(servicos);
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