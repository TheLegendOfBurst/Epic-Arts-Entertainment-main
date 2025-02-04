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

    }
}