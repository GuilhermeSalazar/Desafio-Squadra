using ProjetoSQL.Data;
using ProjetoSQL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoSQL.Repository
{
    public class cursorepository : IRepository
    {
        private readonly Context _context;

        public cursorepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<CursosModel> ObterCursosPorStatus(StatusEnum Status)
        {

            return _context.Cursos.Where((CursosModel course) => course.Status == Status);

        }

        public IEnumerable<CursosModel> ObterTodosCursos()
        {
            return _context.Cursos.ToList();

        }
            
        }
    }

