using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Epic_Arts_Entertainment.Repositorios
{
    public class AgendamentoRepositorio
    {
        private BdEpicArtsEntertainmentContext _context;
        public AgendamentoRepositorio(BdEpicArtsEntertainmentContext context)
        {
            _context = context;
        }

        public bool InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int IdUsuario, int IdServico)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                var atendimento = new TbAgendamento
                {
                    DtHoraAgendamento = dtHoraAgendamento,
                    DataAgendamento = dataAgendamento,
                    Horario = horario,
                    IdUsuario = IdUsuario,
                    IdServico = IdServico
                };

                // Adicionando o atendimento ao contexto
                _context.TbAgendamentos.Add(atendimento);
                _context.SaveChanges(); // Persistindo as mudanças no banco de dados

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                // Em caso de erro, pode-se logar a exceção (ex.Message)
                return false; // Retorna false em caso de erro
            }
        }

        // Método para atualizar um atendimento
        public bool AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {
            try
            {
                TbAgendamento agt = _context.TbAgendamentos.Find(id);
                DateOnly dtHoraAgendamento;
                if (agt != null)
                {
                    agt.IdAgendamento = id;
                    if (data != null)
                    {
                        if (DateOnly.TryParse(data, out dtHoraAgendamento))
                        {
                            agt.DataAgendamento = dtHoraAgendamento;
                        }
                    }

                    // Corrigido a verificação do tipo TimeOnly
                    if (horario != TimeOnly.MinValue)  // Verificando se o horário não é o valor padrão
                    {
                        agt.Horario = horario;
                    }

                    agt.IdServico = servico;
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool ExcluirAgendamento(int id)
        {
            try
            {


                var agt = _context.TbAgendamentos.Where(a => a.IdAgendamento == id).FirstOrDefault();
                if (agt != null)
                {
                    _context.TbAgendamentos.Remove(agt);

                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception)
            {

                return false;
            }
        }

        public List<AgendamentoVM> ListarAgendamentos()
        {
            var listAte = new List<AgendamentoVM>();

            // Inclui a navegação para carregar dados do usuário (e-mail, telefone)
            var listTb = _context.TbAgendamentos
                .Include(a => a.IdUsuarioNavigation)  // Inclui a navegação para o usuário
                .ToList();

            foreach (var item in listTb)
            {
                var agendamento = new AgendamentoVM
                {
                    IdAgendamento = item.IdAgendamento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAgendamento = item.DataAgendamento,
                    Horario = item.Horario,
                    // Dados do Usuário pela navegação
                    UsuarioNome = item.IdUsuarioNavigation.Nome,   // Nome do usuário
                    UsuarioEmail = item.IdUsuarioNavigation.Email, // E-mail do usuário
                    UsuarioTelefone = item.IdUsuarioNavigation.Telefone, // Telefone do usuário
                    ServicoNome = item.IdServicoNavigation.TipoServico,   // Nome do serviço
                    ServicoValor = item.IdServicoNavigation.Valor  // Valor do serviço
                };

                listAte.Add(agendamento);
            }

            return listAte;
        }

        public List<AgendamentoVM> ListarAgendamentosClientes()
        {
            // Obtendo o ID do usuário a partir da variável de ambiente
            string nome = Environment.GetEnvironmentVariable("USUARIO_NOME");

            var listAtendimentos = new List<AgendamentoVM>();

            // Inclui a navegação para carregar dados do usuário (e-mail, telefone)
            var listTb = _context.TbAgendamentos
                .Include(a => a.IdUsuarioNavigation) // Inclui a navegação para o usuário
                .Include(a => a.IdServicoNavigation) // Inclui a navegação para o serviço
                .Where(x => x.IdUsuarioNavigation.Nome == nome)
                .ToList();

            foreach (var item in listTb)
            {
                var agendamento = new AgendamentoVM
                {
                    IdAgendamento = item.IdAgendamento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAgendamento = item.DataAgendamento,
                    Horario = item.Horario,
                    UsuarioNome = item.IdUsuarioNavigation != null ? item.IdUsuarioNavigation.Nome : "N/A", // Se nulo, atribui "N/A"
                    UsuarioEmail = item.IdUsuarioNavigation != null ? item.IdUsuarioNavigation.Email : "N/A",
                    UsuarioTelefone = item.IdUsuarioNavigation != null ? item.IdUsuarioNavigation.Telefone : "N/A",
                    ServicoNome = item.IdServicoNavigation != null ? item.IdServicoNavigation.TipoServico : "N/A",
                    ServicoValor = item.IdServicoNavigation != null ? item.IdServicoNavigation.Valor : 0 // Valor 0 se nulo
                };

                listAtendimentos.Add(agendamento);
            }



            return listAtendimentos;
        }

        public List<AgendamentoVM> ConsultarAgendamento(string datap)
        {
            DateOnly data = DateOnly.ParseExact(datap, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dataFormatada = data.ToString("yyyy-MM-dd"); // Formato desejado: "yyyy-MM-dd"
            Console.WriteLine(dataFormatada);

            try
            {
                // Consulta ao banco de dados, convertendo para o tipo AtendimentoVM
                var ListaAgendamento = _context.TbAgendamentos
                    .Where(a => a.DataAgendamento == DateOnly.Parse(dataFormatada))
                    .Select(a => new AgendamentoVM
                    {
                        // Mapear as propriedades de TbAtendimento para AtendimentoVM
                        // Suponha que TbAtendimento tenha as propriedades Id, DataAtendimento, e outras:
                        IdAgendamento = a.IdAgendamento,
                        DtHoraAgendamento = a.DtHoraAgendamento,
                        DataAgendamento = DateOnly.Parse(dataFormatada),
                        Horario = a.Horario,
                        IdUsuario = a.IdUsuario,
                        IdServico = a.IdServico
                    })
                    .ToList(); // Converte para uma lista

                return ListaAgendamento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar agendamentos: {ex.Message}");
                return new List<AgendamentoVM>(); // Retorna uma lista vazia em caso de erro
            }
        }

        public List<UsuarioVM> ListarNomesAgendamentos()
        {
            // Lista para armazenar os usuários com apenas Id e Nome
            List<UsuarioVM> listFun = new List<UsuarioVM>();

            // Obter apenas os campos Id e Nome da tabela TbUsuarios
            var listTb = _context.TbUsuarios
                                 .Select(u => new UsuarioVM
                                 {
                                     IdUsuario = u.IdUsuario,
                                     Nome = u.Nome
                                 })
                                 .ToList();

            // Retorna a lista já com os campos filtrados
            return listTb;
        }
    }
}