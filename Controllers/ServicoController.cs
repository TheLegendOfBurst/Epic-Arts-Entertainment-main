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

    // Passar os serviços para a view
    return View(servicos);
}


        public IActionResult InserirServico(string tipoServico, decimal valor)
        {
            try
            {
                // Chama o método do repositório que realiza a inserção no banco de dados
                var resultado = _servicoRepositorio.InserirServico(tipoServico, valor);

                // Verifica o resultado da inserção
                if (resultado)
                {
                    // Se o resultado for verdadeiro, significa que o servico foi inserido com sucesso
                    return Json(new { success = true, message = "Servico inserido com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, significa que houve um erro ao tentar inserir o servico
                    return Json(new { success = false, message = "Erro ao inserir o servico. Tente novamente." });
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