using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using System;
using System.Collections.Generic;
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

        public bool InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int idUsuario, int idServico)
        {
            try
            {
                var agendamento = new TbAgendamento
                {
                    DtHoraAgendamento = dtHoraAgendamento,
                    DataAgendamento = dataAgendamento,
                    Horario = horario,
                    IdUsuario = idUsuario,
                    IdServico = idServico
                };

                _context.TbAgendamentos.Add(agendamento);
                _context.SaveChanges();

                return true; // Retorna true para indicar sucesso
            }
            catch (Exception ex)
            {
                // Trate o erro ou faça um log do ex.Message se necessário
                return false; // Retorna false para indicar falha
            }
        }

        public List<AgendamentoVM> ListarAgendamentos()
        {
            List<AgendamentoVM> listAgendamentos = new List<AgendamentoVM>();

            var listTb = _context.TbAgendamentos.ToList();

            foreach (var item in listTb)
            {
                var agendamento = new AgendamentoVM
                {
                    IdAgendamento = item.IdAgendamento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAgendamento = item.DataAgendamento,
                    Horario = item.Horario,
                    IdUsuario = item.IdUsuario,
                    IdServico = item.IdServico
                };

                listAgendamentos.Add(agendamento);
            }

            return listAgendamentos;
        }

        public bool AtualizarAgendamento(int id, DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int idUsuario, int idServico)
        {
            try
            {
                var agendamento = _context.TbAgendamentos.FirstOrDefault(a => a.IdAgendamento == id);
                if (agendamento != null)
                {
                    agendamento.DtHoraAgendamento = dtHoraAgendamento;
                    agendamento.DataAgendamento = dataAgendamento;
                    agendamento.Horario = horario;
                    agendamento.IdUsuario = idUsuario;
                    agendamento.IdServico = idServico;

                    _context.SaveChanges();

                    return true; // Retorna verdadeiro se a atualização for bem-sucedida
                }
                else
                {
                    return false; // Retorna falso se o agendamento não foi encontrado
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o agendamento com ID {id}: {ex.Message}");
                return false;
            }
        }

        public bool ExcluirAgendamento(int id)
        {
            try
            {
                var agendamento = _context.TbAgendamentos.FirstOrDefault(a => a.IdAgendamento == id);
                if (agendamento == null)
                {
                    throw new KeyNotFoundException("Agendamento não encontrado.");
                }

                _context.TbAgendamentos.Remove(agendamento);
                _context.SaveChanges(); // Isso pode lançar uma exceção se houver dependências

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir o agendamento com ID {id}: {ex.Message}");
                throw new Exception($"Erro ao excluir o agendamento: {ex.Message}");
            }
        }
    }
}