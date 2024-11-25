using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using Microsoft.EntityFrameworkCore;
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

        public void Add(AgendamentoVM agendamento)
        {
            var tbAgendamento = new TbAgendamento()
            {
                DtHoraAgendamento = agendamento.DtHoraAgendamento,
                DataAgendamento = agendamento.DataAgendamento,
                Horario = agendamento.Horario,
                IdUsuario = agendamento.IdUsuario,  // Vincula o usuário
                IdServico = agendamento.IdServico   // Vincula o serviço
            };

            _context.TbAgendamentos.Add(tbAgendamento);
            _context.SaveChanges();
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