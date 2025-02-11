using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using Microsoft.EntityFrameworkCore;
using Highsoft.Web.Mvc.Charts;
using System.Collections.Generic;

namespace Epic_Arts_Entertainment.Repositorios
{
    public class DashboardRepositorio
    {
        private BdEpicArtsEntertainmentContext _context;


        public DashboardRepositorio(BdEpicArtsEntertainmentContext context)
        {
            _context = context;

        }

        public List<LineSeriesData> ObterDadosGrafico()
        {
            return new List<LineSeriesData>
                     {
                         new LineSeriesData { Y = 10 },
                         new LineSeriesData { Y = 25 },
                         new LineSeriesData { Y = 35 },
                         new LineSeriesData { Y = 50 }
                     };
        }
        public int ContarAgendamentosPorAno(int ano)
        {
            return _context.TbAgendamentos
                .Where(a => a.DtHoraAgendamento.Year == ano)
                .Count();
        }
        public int ContarUsuariosPorAno(int ano)
        {
            return _context.TbUsuarios
                           .Where(u => u.DataHoraCadastro.Year == ano)
                           .Count();
        }
        public decimal SomarLucroPorAno(int ano)
        {
            var lucroTotal = _context.ViewAgendamentos
                                     .Where(a => a.DtHoraAgendamento.Year == ano)
                                     .Sum(a => (decimal?)a.Valor) ?? 0;

            return lucroTotal;
        }

        public List<AgendamentosPorMes> ContarAgendamentosPorMes(int ano)
        {
            return _context.TbAgendamentos
                           .Where(a => a.DtHoraAgendamento.Year == ano)
                           .GroupBy(a => a.DtHoraAgendamento.Month)
                           .OrderBy(g => g.Key)
                           .Select(g => new AgendamentosPorMes
                           {
                               Mes = g.Key,  // Mês (1 para Janeiro, 2 para Fevereiro, etc.)
                               TotalAgendamentos = g.Count()  // Contagem de agendamentos nesse mês
                           })
                           .ToList();
        }

        public List<UsuariosPorMes> ContarUsuariosPorMes(int ano)
        {
            return _context.TbUsuarios
                           .Where(u => u.DataHoraCadastro.Year == ano)
                           .GroupBy(u => u.DataHoraCadastro.Month)
                           .OrderBy(g => g.Key)
                           .Select(g => new UsuariosPorMes
                           {
                               Mes = g.Key,  // Mês (1 para Janeiro, 2 para Fevereiro, etc.)
                               TotalUsuarios = g.Count()  // Contagem de usuários cadastrados nesse mês
                           })
                           .ToList();
        }

        public List<decimal> SomarLucroPorMes(int ano)
        {
            return _context.ViewAgendamentos
                           .Where(a => a.DtHoraAgendamento.Year == ano)
                           .GroupBy(a => a.DtHoraAgendamento.Month)
                           .OrderBy(g => g.Key)
                           .Select(g => g.Sum(a => (decimal?)a.Valor) ?? 0)
                           .ToList();
        }

    }
}