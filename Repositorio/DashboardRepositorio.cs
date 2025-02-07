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
    }
}