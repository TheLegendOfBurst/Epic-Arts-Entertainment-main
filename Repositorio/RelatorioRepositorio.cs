using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using Microsoft.EntityFrameworkCore;

namespace Epic_Arts_Entertainment.Repositorios
{
    public class RelatorioRepositorio
    {

        private readonly BdEpicArtsEntertainmentContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Construtor
        public RelatorioRepositorio(BdEpicArtsEntertainmentContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;  // Injeção de IHttpContextAccessor
        }

        public List<ViewAgendamento> GetAgendamentos(
            string campo1, string campo2, string campo3,
            string valor1, string valor2, string valor3)
        {
            if (string.IsNullOrEmpty(campo1) && string.IsNullOrEmpty(campo2) && string.IsNullOrEmpty(campo3) &&
                string.IsNullOrEmpty(valor1) && string.IsNullOrEmpty(valor2) && string.IsNullOrEmpty(valor3))
            {
                return _context.ViewAgendamentos
                    .OrderBy(a => a.DtHoraAgendamento)
                    .Take(1000)
                    .ToList();
            }

            var query = _context.ViewAgendamentos.AsQueryable();

            // Filtro para campo1 e valor1
            if (!string.IsNullOrEmpty(campo1) && !string.IsNullOrEmpty(valor1))
            {
                if (campo1.Equals("TipoServico", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(a => a.TipoServico.ToLower() == valor1.ToLower());
                else if (campo1.Equals("Nome", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(a => a.Nome.ToLower() == valor1.ToLower());
                else if (campo1.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(a => a.Email.ToLower() == valor1.ToLower());
                else if (campo1.Equals("Telefone", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(a => a.Telefone.ToLower() == valor1.ToLower());
                else if (campo1.Equals("Valor", StringComparison.OrdinalIgnoreCase))
                {
                    // Certifique-se de comparar os valores corretamente
                    if (decimal.TryParse(valor1, out decimal valor))
                    {
                        query = query.Where(a => a.Valor == valor); // Comparação correta com '=='
                    }
                }
                else if (campo1.Equals("DataAtendimento", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParse(valor1, out DateTime dateValue))
                    {
                        // Converter para DateOnly para comparar apenas as datas
                        DateOnly dateOnlyValue = DateOnly.FromDateTime(dateValue);

                        query = query.Where(a => a.DataAgendamento == dateOnlyValue);
                    }
                }
                else if (campo1.Equals("Horario", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParse(valor1, out DateTime timeValue))
                        query = query.Where(a => a.Horario.Hour == timeValue.Hour && a.DtHoraAgendamento.Minute == timeValue.Minute);
                }
            }

            // Filtro para campo2 e valor2
            if (!string.IsNullOrEmpty(campo2) && !string.IsNullOrEmpty(valor2))
            {
                // Adicione os mesmos filtros para campo2 (como foi feito para campo1)
            }

            // Filtro para campo3 e valor3
            if (!string.IsNullOrEmpty(campo3) && !string.IsNullOrEmpty(valor3))
            {
                // Adicione os mesmos filtros para campo3 (como foi feito para campo1)
            }

            var agendamentos = query
                .OrderBy(a => a.DtHoraAgendamento)
                .Take(1000)
                .ToList();

            return agendamentos;
        }
    }
}