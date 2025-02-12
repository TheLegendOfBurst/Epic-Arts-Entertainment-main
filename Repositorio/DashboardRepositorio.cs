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

        public IEnumerable<AgendamentosPorMes> ContarAgendamentosPorMes(int ano)
        {
            return _context.TbAgendamentos
                .Where(a => a.DtHoraAgendamento.Year == ano)
                .GroupBy(a => a.DtHoraAgendamento.Month)
                .OrderBy(g => g.Key)
                .Select(g => new AgendamentosPorMes
                {
                    Mes = g.Key,
                    TotalAgendamentos = g.Count()
                })
                .ToList();
        }
        public IEnumerable<UsuariosPorMes> ContarUsuariosPorMes(int ano)
        {
            return _context.TbUsuarios
                .Where(u => u.DataHoraCadastro.Year == ano)
                .GroupBy(u => u.DataHoraCadastro.Month)
                .OrderBy(g => g.Key)
                .Select(g => new UsuariosPorMes
                {
                    Mes = g.Key,
                    TotalUsuarios = g.Count()
                })
                .ToList();
        }
        public IEnumerable<LucroPorMes> SomarLucroPorMes(int ano)
        {
            return _context.ViewAgendamentos
                .Where(a => a.DtHoraAgendamento.Year == ano)
                .GroupBy(a => a.DtHoraAgendamento.Month)
                .Select(g => new LucroPorMes
                {
                    Mes = g.Key,
                    TotalLucro = g.Sum(a => (decimal?)a.Valor) ?? 0
                })
                .ToList();
        }

    }
}