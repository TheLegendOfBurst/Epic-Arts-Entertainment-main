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

        public void Inserir(TbAgendamento agendamento)
        {
            _context.TbAgendamentos.Add(agendamento); // Adiciona o agendamento à tabela
            _context.SaveChanges(); // Salva as alterações no banco de dados
        }


        public void Delete(int id)
        {
            var tbAgendamento = _context.TbAgendamentos.FirstOrDefault(f => f.IdAgendamento == id);
            if (tbAgendamento != null)
            {
                _context.TbAgendamentos.Remove(tbAgendamento);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Agendamento não encontrado.");
            }
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

        public void Update(AgendamentoVM agendamento)
        {
            var tbAgendamento = _context.TbAgendamentos.FirstOrDefault(f => f.IdAgendamento == agendamento.IdAgendamento);
            if (tbAgendamento != null)
            {
                tbAgendamento.DtHoraAgendamento = agendamento.DtHoraAgendamento;
                tbAgendamento.DataAgendamento = agendamento.DataAgendamento;
                tbAgendamento.Horario = agendamento.Horario;
                tbAgendamento.IdUsuario = agendamento.IdUsuario;
                tbAgendamento.IdServico = agendamento.IdServico;

                _context.TbAgendamentos.Update(tbAgendamento);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Agendamento não encontrado.");
            }
        }
    }
}